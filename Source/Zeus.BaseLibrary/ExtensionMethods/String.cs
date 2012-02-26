using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Zeus.BaseLibrary.ExtensionMethods
{
	public static class StringExtensionMethods
	{
		public static bool Contains(this string thisString, string value, StringComparison comparisonType)
		{
			return (thisString.IndexOf(value, comparisonType) != -1);
		}

		public static string Left(this string value, int length)
		{
			return (length >= value.Length) ? value : value.Substring(0, length);
		}

		public static bool IsSafeUrl(this string value)
		{
			return value == value.ToSafeUrl();
		}

		public static string ToSafeUrl(this string value)
		{
			if (string.IsNullOrEmpty(value))
				return string.Empty;

            string temp = value.ToLower().Trim();
            //temp = temp.Replace("  ", " ").Replace("  ", " ").Trim();
			temp = Regex.Replace(temp, "[ ]+", "-");

			string result = string.Empty;
			foreach (char c in temp)
			{
				switch (CharUnicodeInfo.GetUnicodeCategory(c))
				{
					case UnicodeCategory.DecimalDigitNumber :
					case UnicodeCategory.LowercaseLetter :
						result += HttpUtility.UrlEncode(c.ToString());
						break;
					default :
						switch (c)
						{
							case '-' :
							case '_' :
								result += c;
								break;
						}
						break;
				}
			}
			return result;
		}

		public static T ToEnum<T>(this string value)
		{
			return (T)Enum.Parse(typeof(T), value);
		}

		public static int LuhnChecksum(this string value)
		{
			return value
			       	.Where(Char.IsDigit)
			       	.Reverse()
			       	.SelectMany((c, i) => ((c - '0') << (i & 1)).ToString())
			       	.Sum(c => c - '0') % 10;
		}
	}
}