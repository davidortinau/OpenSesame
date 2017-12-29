using System;
using System.Collections.Generic;
using OpenSesame.ViewModels;
using Xamarin.Forms;

namespace OpenSesame.Views
{
	public partial class SettingsView : ContentPage
	{
		public SettingsView()
		{
			InitializeComponent();

			BindingContext = new SettingsViewModel(){
				Nav = Navigation
			};
		}
	}
}
