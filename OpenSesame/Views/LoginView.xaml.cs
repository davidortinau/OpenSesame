using System;
using System.Collections.Generic;
using OpenSesame.ViewModels;
using Xamarin.Forms;

namespace OpenSesame.Views
{
	public partial class LoginView : ContentPage
	{
		public LoginView()
		{
			InitializeComponent();

			BindingContext = new LoginViewModel(){
				Nav = this.Navigation
			};
		}
	}
}
