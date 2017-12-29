using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using MyQNET.Enums;
using OpenSesame.Services;
using OpenSesame.Store;
using OpenSesame.ViewModels;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace OpenSesame.Views
{
	public partial class MainView : ReactiveContentPage<MainViewModel>
	{
		BingSpeechService bingSpeechService;

		public MainView()
		{
			InitializeComponent();

			ViewModel = new MainViewModel()
			{
				Nav = this.Navigation
			};

			bingSpeechService = new BingSpeechService(new AuthenticationService(ApplicationStore.BingSpeechAPIKey), Device.RuntimePlatform);

			this
				.WhenAnyValue(x => x.ViewModel.DoorState)
				.Subscribe(doorState =>
				{
					if (doorState == DoorState.Open || doorState == DoorState.GoingUp)
					{
						BotBody.TranslateTo(BotBody.X, BotBody.Y, 2000, Easing.SinIn);
					}
					else if (doorState == DoorState.Closed || doorState == DoorState.GoingDown)
					{
						BotBody.TranslateTo(BotBody.X, BotBody.Y + 100, 2000, Easing.SinIn);
					}
				});

			this
				.WhenActivated(
					disposables =>
			{
				this.OneWayBind(ViewModel, vm => vm.DoorStateDescription,
											 c => c.DoorStateLabel.Text)
						.DisposeWith(disposables);

				this.BindCommand(ViewModel, vm => vm.GetDevicesCommand,
												c => c.RefreshButton)
						.DisposeWith(disposables);

				this.OneWayBind(ViewModel, vm => vm.OpenCloseButtonTitle,
											 c => c.OpenCloseButton.Text)
						.DisposeWith(disposables);

				this.Bind(ViewModel, vm => vm.IsDoorAvailable,
								 c => c.VoiceButton.IsEnabled)
						.DisposeWith(disposables);

				this.BindCommand(ViewModel, vm => vm.ToggleDoorCommand,
												c => c.OpenCloseButton)
						.DisposeWith(disposables);

				this.BindCommand(ViewModel, vm => vm.SettingsCommand,
												c => c.SettingsBtn)
						.DisposeWith(disposables);
			});
		}

		bool isRecording;
		bool isProcessing;
		async void OnRecognizeSpeechButtonClicked(object sender, EventArgs e)
		{
			try
			{
				var audioRecordingService = DependencyService.Get<IAudioRecorderService>();
				if (!isRecording)
				{
					audioRecordingService.StartRecording();

					((Button)sender).Text = "Recording";
					isProcessing = true;
				}
				else
				{
					audioRecordingService.StopRecording();
				}

				isRecording = !isRecording;
				if (!isRecording)
				{
					((Button)sender).Text = "Processing";

					var speechResult = await bingSpeechService.RecognizeSpeechAsync(Constants.AudioFilename);
					Debug.WriteLine("Name: " + speechResult.Name);
					Debug.WriteLine("Confidence: " + speechResult.Confidence);

					if (!string.IsNullOrWhiteSpace(speechResult.Name))
					{
						// TODO action
						ViewModel.VoiceCommand.Execute(speechResult.Name);
						Debug.WriteLine(char.ToUpper(speechResult.Name[0]) + speechResult.Name.Substring(1));
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
			finally
			{
				if (!isRecording)
				{
					((Button)sender).Text = "Voice Command";
					//((Button)sender).Image = "record.png";
					isProcessing = false;
				}
			}
		}
	}
}
