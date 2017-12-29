using System;
using System.Text.RegularExpressions;

namespace Rendr.XFKit.Core.Util
{
	public static class RegexUtil
	{
		public static Regex ValidEmailAddress()
		{
			return new Regex(@"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
+ "@"
			                       + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$");
		}

		public static Regex MinLength(int length)
		{
			return new Regex(@"(\s*(\S)\s*){" + length + @",}");
		}

		public static Regex ValidZipCode()
		{
			return new Regex(@"^\d{5}(?:[-\s]\d{4})?$");
		}
	}
}

