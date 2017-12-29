using System;
using System.Windows.Input;
using OpenSesame.Store;
using Xamarin.Forms;

namespace OpenSesame.ViewModels
{
	public class SettingsViewModel
	{
		public string CognitiveServicesEndpoint {get;set;} = ApplicationStore.CognitiveServicesEndpoint;

		public string BingSpeechRecognitionEndpoint {get;set;} = ApplicationStore.BingSpeechRecognitionEndpoint;
		
		public string BingSpeechAPIKey {get;set;} = ApplicationStore.BingSpeechAPIKey;

		public INavigation Nav {get;set;}

		public ICommand SaveCommand {get;set;}

		public ICommand BackCommand {get;set;}

		public ICommand LogoutCommand {get;set;}

		public SettingsViewModel()
		{
			SaveCommand = new Command(OnSave);
			BackCommand = new Command(async ()=> await Nav.PopModalAsync());
			LogoutCommand = new Command(OnLogout);
		}

		private async void OnLogout()
		{
			ApplicationStore.Logout();
			await Nav.PopModalAsync();
		}

		private async void OnSave()
		{
			ApplicationStore.CognitiveServicesEndpoint = CognitiveServicesEndpoint;
			ApplicationStore.BingSpeechRecognitionEndpoint = BingSpeechRecognitionEndpoint;
			ApplicationStore.BingSpeechAPIKey = BingSpeechAPIKey;

			await Nav.PopModalAsync();
		}
	}
}
