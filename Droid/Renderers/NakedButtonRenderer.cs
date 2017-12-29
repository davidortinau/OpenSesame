using System;
using OpenSesame.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;

[assembly: ExportRenderer(typeof(Button), typeof(NakedButtonRenderer))] 
namespace OpenSesame.Droid.Renderers
{
	public class NakedButtonRenderer : ButtonRenderer
	{
		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Button> e)
		{
			base.OnElementChanged(e);

			Control.StateListAnimator = null; // removes shadow
		}
	}
}
