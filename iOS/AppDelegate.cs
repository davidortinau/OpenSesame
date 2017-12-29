using System;
using System.Collections.Generic;
using System.Linq;
using FFImageLoading.Forms.Touch;
using FFImageLoading.Svg.Forms;
using Foundation;
using Intents;

using UIKit;

namespace OpenSesame.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			CachedImageRenderer.Init();
			var ignore = typeof(SvgCachedImage);
			LoadApplication(new App());

			// Request access to Siri
			//INPreferences.RequestSiriAuthorization ((INSiriAuthorizationStatus status) => {
			//	// Respond to returned status
			//	switch (status) {
			//		case INSiriAuthorizationStatus.Authorized:
			//			break;
			//		case INSiriAuthorizationStatus.Denied:
			//			break;
			//		case INSiriAuthorizationStatus.NotDetermined:
			//			break;
			//		case INSiriAuthorizationStatus.Restricted:
			//			break;
			//	}
			//});

			return base.FinishedLaunching(app, options);
		}
	}
}
