using MyQNET;
using OpenSesame.Models;
using Xamarin.Forms;
using OpenSesame.Views;

namespace OpenSesame
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			DependencyService.Register<IAppModel, AppModel>();
			DependencyService.Register<IMyQService, MyQService>();

			MainPage = new NavigationPage(new MainView()){
				StyleId = ".NavPage"
			};
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
