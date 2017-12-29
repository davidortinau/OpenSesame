using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MyQNET;
using OpenSesame.Store;
using OpenSesame.Utils;
using OpenSesame.Views;
using Rendr.XFKit.Core.Util;
using Xamarin.Forms;

namespace OpenSesame.ViewModels
{
	public class LoginViewModel : INotifyPropertyChanged
	{
		string _username = ApplicationStore.Username;
		public string Username {
			get{return _username;}
			set{
				_username = value;
				if(RegexUtil.ValidEmailAddress().Match(_username).Success)
					OnPropertyChanged(nameof(ProfilePhotoUrl));
			}
		}

		public string Password {get;set;} = ApplicationStore.Password;

		public ICommand SettingsCommand { get; set; }
		public ICommand LoginCommand {get;set;}
		public INavigation Nav {get;set;}

		IMyQService _svc;

		public LoginViewModel()
		{
			_svc = DependencyService.Get<IMyQService>(DependencyFetchTarget.GlobalInstance);

			LoginCommand = new Command(OnLogin);
			SettingsCommand = new Command(async () => await Nav.PushModalAsync(new SettingsView()));
		}

		Uri profilePhotoUrl;
		public Uri ProfilePhotoUrl
		{
			get
			{
				if(RegexUtil.ValidEmailAddress().Match(_username).Success)
				{
					profilePhotoUrl = Gravatar.GetImageUrl(_username, 88*2);
				}
				return profilePhotoUrl;
			}

		}

		async void OnLogin()
		{
			ApplicationStore.Username = Username;
			ApplicationStore.Password = Password;

			(var success, var token) = await _svc.Login(Username, Password);

			if(success)
			{
				ApplicationStore.SecurityToken = token;
				await Nav.PopModalAsync();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}
