using System;
using OpenSesame.Utils;
using Xamarin.Forms;
using OpenSesame;

namespace OpenSesame.Store
{
	public static class ApplicationStore
	{
		public static string SecurityToken 
		{
			get {
				if(Application.Current.Properties.ContainsKey(nameof(SecurityToken)))
				{
					return Application.Current.Properties[nameof(SecurityToken)].ToString();
				}else{
					return string.Empty;
				}
			}
			set 
			{
				if(Application.Current.Properties.ContainsKey(nameof(SecurityToken)))
				{
					Application.Current.Properties[nameof(SecurityToken)] = value;
				}else{
					Application.Current.Properties.Add(nameof(SecurityToken), value);
				}
				Application.Current.SavePropertiesAsync();
			}
		}

		public static string Username
		{
			get {
				if(Application.Current.Properties.ContainsKey(nameof(Username)))
				{
					var bytes = Application.Current.Properties[nameof(Username)].ToString();
					return Crypt.Decrypt(bytes, Constants.StoreKey);
				}else{
					return string.Empty;
				}
			}
			set {
				if(Application.Current.Properties.ContainsKey(nameof(Username)))
				{
					Application.Current.Properties[nameof(Username)] = Crypt.Encrypt(value, Constants.StoreKey);
				}else{
					Application.Current.Properties.Add(nameof(Username), Crypt.Encrypt(value, Constants.StoreKey));
				}
				Application.Current.SavePropertiesAsync();
			}
		}

		public static string Password
		{
			get {
				if(Application.Current.Properties.ContainsKey(nameof(Password)))
				{
					var bytes = Application.Current.Properties[nameof(Password)].ToString();
					return Crypt.Decrypt(bytes, Constants.StoreKey);
				}else{
					return string.Empty;
				}
			}
			set {
				if(Application.Current.Properties.ContainsKey(nameof(Password)))
				{
					Application.Current.Properties[nameof(Password)] = Crypt.Encrypt(value, Constants.StoreKey);
				}else{
					Application.Current.Properties.Add(nameof(Password), Crypt.Encrypt(value, Constants.StoreKey));
				}
				Application.Current.SavePropertiesAsync();
			}
		}

		public static string CognitiveServicesEndpoint {
			get {
				if(Application.Current.Properties.ContainsKey(nameof(CognitiveServicesEndpoint)))
				{
					return Application.Current.Properties[nameof(CognitiveServicesEndpoint)].ToString();;
				}else{
					return Constants.CognitiveServicesEndpoint;
				}
			}
			set {
				if(Application.Current.Properties.ContainsKey(nameof(CognitiveServicesEndpoint)))
				{
					Application.Current.Properties[nameof(CognitiveServicesEndpoint)] = value;
				}else{
					Application.Current.Properties.Add(nameof(CognitiveServicesEndpoint), value);
				}
				Application.Current.SavePropertiesAsync();
			}
		}

		public static string BingSpeechRecognitionEndpoint {
			get {
				if(Application.Current.Properties.ContainsKey(nameof(BingSpeechRecognitionEndpoint)))
				{
					return Application.Current.Properties[nameof(BingSpeechRecognitionEndpoint)].ToString();;
				}else{
					return Constants.BingSpeechRecognizeEndpoint;
				}
			}
			set {
				if(Application.Current.Properties.ContainsKey(nameof(BingSpeechRecognitionEndpoint)))
				{
					Application.Current.Properties[nameof(BingSpeechRecognitionEndpoint)] = value;
				}else{
					Application.Current.Properties.Add(nameof(BingSpeechRecognitionEndpoint), value);
				}
				Application.Current.SavePropertiesAsync();
			}
		}

		public static string BingSpeechAPIKey {
			get {
				if(Application.Current.Properties.ContainsKey(nameof(BingSpeechAPIKey)))
				{
					return Application.Current.Properties[nameof(BingSpeechAPIKey)].ToString();
				}else{
					return string.Empty;
				}
			}
			set {
				if(Application.Current.Properties.ContainsKey(nameof(BingSpeechAPIKey)))
				{
					Application.Current.Properties[nameof(BingSpeechAPIKey)] = value;
				}else{
					Application.Current.Properties.Add(nameof(BingSpeechAPIKey), value);
				}
				Application.Current.SavePropertiesAsync();
			}
		}

		public static void Logout()
		{
			if(Application.Current.Properties.ContainsKey(nameof(Username)))
				Application.Current.Properties.Remove(nameof(Username));

			if(Application.Current.Properties.ContainsKey(nameof(Password)))
				Application.Current.Properties.Remove(nameof(Password));

			if(Application.Current.Properties.ContainsKey(nameof(SecurityToken)))
				Application.Current.Properties.Remove(nameof(SecurityToken));
		}
	}
}