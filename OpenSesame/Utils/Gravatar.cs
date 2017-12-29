using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace OpenSesame.Utils
{
	public class Gravatar
	{
		static JeffWilcox.Utilities.Silverlight.MD5 md5;

		static Gravatar()
		{
			// MD5 not available in PCL, using an open-source implementation
			md5 = JeffWilcox.Utilities.Silverlight.MD5.Create("MD5");
		}

		public static Uri GetImageUrl(string email, int size)
		{
			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentException("Email must be a valid email address.", "email");
			}
			if (size <= 0)
			{
				throw new ArgumentException("Size must be greater than 0.", "size");
			}

			var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(email.Trim()));
			//var hashString = string.Join("", hash.Select(x => x.ToString("x2")));
			StringBuilder hashString = new StringBuilder();
			for (int i = 0; i < hash.Length; i++) hashString.Append(hash[i].ToString("x2"));
			Debug.WriteLine($"https://www.gravatar.com/avatar/{hashString}?s=120&d=mm");
			return new Uri($"https://www.gravatar.com/avatar/{hashString}?s=120&d=mm");
		}
	}
}