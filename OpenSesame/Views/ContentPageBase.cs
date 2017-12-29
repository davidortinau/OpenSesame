using System;
using Xamarin.Forms;
using ReactiveUI;

namespace OpenSesame.Views
{
	public class ContentPageBase<TViewModel> : ContentPage, IViewFor<TViewModel> where TViewModel : class
	{
		public ContentPageBase ()
		{

			//this.WhenActivated(d =>
			//{
			//	// This will be called
			//});
		}

		public static readonly BindableProperty ViewModelProperty = 
			BindableProperty.Create(nameof(ViewModels), typeof(TViewModel), typeof(ContentPageBase<TViewModel>), default(TViewModel), BindingMode.OneWay);

		#region IViewFor implementation

		public TViewModel ViewModel {
			get {
				return (TViewModel)GetValue (ViewModelProperty);
			}
			set {
				SetValue (ViewModelProperty, value);
			}
		}

		#endregion

		#region IViewFor implementation

		object IViewFor.ViewModel {
			get {
				return ViewModel;
			}
			set {
				ViewModel = (TViewModel)value;
			}
		}

		#endregion
	}
}
