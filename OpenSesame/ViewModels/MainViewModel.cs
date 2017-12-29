using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using OpenSesame.Views;
using Xamarin.Forms;
using Device = MyQNET.Models.Device;
using OpenSesame.Store;
using System.Threading.Tasks;
using MyQNET;
using MyQNET.Enums;
using EnumsNET;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;

namespace OpenSesame.ViewModels
{
	public class MainViewModel : ReactiveObject, ISupportsActivation
	{
		readonly ViewModelActivator activator;

		public ViewModelActivator Activator => this.activator;

		IMyQService myQ;

		string doorStateDescription = DoorState.Unknown.ToString();

		DoorState doorState;

		public INavigation Nav { get; set; }

		public ReactiveCommand<object, Unit> GetDevicesCommand;

		public ReactiveCommand<object, Unit> ToggleDoorCommand { get; set; }

		public ReactiveCommand<object, Unit> SettingsCommand { get; set; }

		public ICommand VoiceCommand { get; set; }

		public string OpenCloseButtonTitle
		{
			get
			{
				return (DoorState == DoorState.Open) ? "Close Door" : "Open Door";
			}
		}

		public string DoorStateDescription
		{
			get { return DoorState.GetMember().Attributes.Get<DescriptionAttribute>().Description; }
		}

		public bool IsDoorOpen
		{
			get { return DoorState == DoorState.Open; }
		}

		public bool HasDevice
		{
			get { return Door != null; }
		}

		readonly ObservableAsPropertyHelper<bool> canToggleDoor;

		public bool CanToggleDoor
		{
			get { return canToggleDoor.Value; }
		}

		public int IsActiveCount { get; protected set; }

		public MainViewModel()
		{
			this.activator = new ViewModelActivator();

			myQ = DependencyService.Get<IMyQService>(DependencyFetchTarget.GlobalInstance);

			canToggleDoor = this.WhenAnyValue(x => x.Door, y => y.DoorState,
																				(x, y) => x != null && (y == DoorState.Open || y == DoorState.Closed))
													.ToProperty(this, x => x.CanToggleDoor);

			GetDevicesCommand = ReactiveCommand.CreateFromTask<object, Unit>(
				async _ =>
				{
					if (myQ.IsAuthenticated)
					{
						var devices = await myQ.GetDevices();
						if (devices == null)
						{
							myQ.SetSecurityToken(string.Empty);
							ApplicationStore.SecurityToken = string.Empty;
							await Nav.PushModalAsync(new LoginView());
						}
						else
						{
							Door = devices.First();
							DoorState = await myQ.GetDoorState(devices.First().MyQDeviceId);
						}

					}
					else
					{
						await Nav.PushModalAsync(new LoginView());
					}
					return Unit.Default;
				});


			var canExecuteToggle =
				this.WhenAnyValue(x => x.CanToggleDoor, (arg) => arg == true);
			
			ToggleDoorCommand = ReactiveCommand.CreateFromTask<object, Unit>(
				async _ =>
				{
					await ToggleDoor();
					return Unit.Default;
				}, canExecuteToggle);

			SettingsCommand = ReactiveCommand.CreateFromTask<object, Unit>(
				async _ =>
				{
					await Nav.PushModalAsync(new SettingsView());
					return Unit.Default;
				}
			);

			VoiceCommand = ReactiveCommand.CreateFromTask<string, Unit>(
				async (phrase) =>
				{
					await ProcessSpeechCommand(phrase);
					return Unit.Default;
				}
			);

			this.WhenActivated(
				d =>
				{
					// this isn't useful but it gets rid of the ambiguous compile error
					IsActiveCount++;
					d(Disposable.Create(() => IsActiveCount--));
	
					if (string.IsNullOrEmpty(ApplicationStore.SecurityToken) && !myQ.IsAuthenticated)
					{
						Nav.PushModalAsync(new LoginView());
					}
					else
					{
						myQ.SetSecurityToken(ApplicationStore.SecurityToken);
						OnGetDevices();
					}
				});
		}

		async Task ProcessSpeechCommand(string phrase)
		{
			if (hasOpenWords(phrase) && doorState == DoorState.Closed)
			{
				await ToggleDoor();
			}
			else if (hasCloseWords(phrase) && doorState == DoorState.Open)
			{
				await ToggleDoor();
			} // otherwise ignore it
		}

		string[] openWords = new string[] { "open", "raise" };
		bool hasOpenWords(string phrase)
		{
			foreach (var word in openWords)
			{
				if (phrase.IndexOf(word) > -1) return true;
			}

			return false;
		}

		string[] closeWords = new string[] { "close", "shut", "lower" };
		bool hasCloseWords(string phrase)
		{
			foreach (var word in closeWords)
			{
				if (phrase.IndexOf(word) > -1) return true;
			}

			return false;
		}

		private async Task ToggleDoor()
		{
			if (doorState == DoorState.Open)
			{
				await CloseDoor();
			}
			else
			{
				await OpenDoor();
			}
		}

		private async Task CloseDoor()
		{
			if (Door != null)
			{
				DoorState = DoorState.GoingDown;
				await myQ.CloseDoor(Door.MyQDeviceId);
				await Task.Delay(4000);
				await PollForDoorStatus(DoorState.Closed);
			}
		}

		private async Task OpenDoor()
		{
			if (Door != null)
			{
				DoorState = DoorState.GoingUp;
				await myQ.OpenDoor(Door.MyQDeviceId);
				await Task.Delay(4000);
				await PollForDoorStatus(DoorState.Open);
			}
		}

		async Task PollForDoorStatus(DoorState destinationState)
		{
			int maxChecks = 30;
			while (doorState != destinationState && maxChecks != 0)
			{
				DoorState = await myQ.GetDoorState(Door.MyQDeviceId);
				await Task.Delay(2000);
				maxChecks--;
			}
		}

		public bool IsDoorAvailable
		{
			get
			{
				return (doorState == DoorState.Open || doorState == DoorState.Closed);
			}
		}

		public DoorState DoorState
		{
			get { return doorState; }
			set
			{
				this.RaiseAndSetIfChanged(ref doorState, value);
				this.RaisePropertyChanged(nameof(DoorStateDescription));
				this.RaisePropertyChanged(nameof(IsDoorOpen));
				this.RaisePropertyChanged(nameof(OpenCloseButtonTitle));
				this.RaisePropertyChanged(nameof(IsDoorAvailable));
				this.RaisePropertyChanged(nameof(CanToggleDoor)); ;
			}
		}

		private Device door;
		public Device Door
		{
			get
			{
				return door;
			}
			set
			{
				this.RaiseAndSetIfChanged(ref door, value);
			}
		}

		public async void OnGetDevices()
		{
			if (myQ.IsAuthenticated)
			{
				var devices = await myQ.GetDevices();
				if (devices == null)
				{
					myQ.SetSecurityToken(string.Empty);
					ApplicationStore.SecurityToken = string.Empty;
					await Nav.PushModalAsync(new LoginView());
				}
				else
				{
					Door = devices.First();
					DoorState = await myQ.GetDoorState(devices.First().MyQDeviceId);
				}

			}
		}
	}
}
