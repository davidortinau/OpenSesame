# Holiday 2017 Experiment 
I broke our 20+ year old garage door opener...pieces flew everwhere in a glorious demise. The new "garage bot" is wifi enabled, so of course I must tinker. It's nothing mind-blowing, but it has been a good excuse to spend some time using a variety of things I've been wanting to explore:

- [Flurl HTTP Client](https://github.com/tmenier/Flurl)
- [SkiaSharp SVG](https://github.com/mono/SkiaSharp)
- Reactive Extensions and [ReactiveUI](https://reactiveui.net/)
- [Xamarin.Forms CSS Stylesheets](https://github.com/xamarin/Xamarin.Forms/pull/1207)
- Azure Cognitive Services and Bing Speech Recognition

![login closed](https://github.com/davidortinau/OpenSesame/blob/master/Screenshots/login-closed.gif)
![opening](https://github.com/davidortinau/OpenSesame/blob/master/Screenshots/opening.gif)

## OpenSesame Garage Door Opener

Talking to the MyQ API. Supported actions:
- Login
- GetDevices
- GetDeviceState
- SetDeviceState (open and close)

> If you want to use the MyQ API and (obviously) have a compatible garage door openeers, setup a Chamberlain or LiftMaster account through their website or app.

## MyQ Resources

https://unofficialliftmastermyq.docs.apiary.io/#

https://www.npmjs.com/package/myq-api

## Azure Cognitive Services and Bing Speech Recognition

https://developer.xamarin.com/guides/xamarin-forms/cloud-services/cognitive-services/speech-recognition/

https://azure.microsoft.com/en-us/services/cognitive-services/

> API Key required. Be sure to get a key from the link above and add it on the Settings view of the app. 

## Other

Xamarin.Forms Nightly Builds - https://github.com/xamarin/Xamarin.Forms/wiki/Nightly-Builds
