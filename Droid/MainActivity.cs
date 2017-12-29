
using Android.App;
using Android.Content.PM;
using Android.OS;
using FFImageLoading.Forms.Droid;
using FFImageLoading.Svg.Forms;

namespace OpenSesame.Droid
{
	[Activity(Label = "OpenSesame.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			CachedImageRenderer.Init(false);
			var ignore = typeof(SvgCachedImage);
			global::Xamarin.Forms.Forms.Init(this, bundle);


			LoadApplication(new App());
		}
	}
}
