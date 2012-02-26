using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Zeus.BaseLibrary
{
	public static class EnumHelper
	{
		private static readonly Dictionary<string, string> CachedEnumDescriptions = new Dictionary<string, string>();

		public static string GetEnumValueDescription(Type enumType, string name)
		{
			string cacheKey = enumType.FullName + "." + name;
			if (!CachedEnumDescriptions.ContainsKey(cacheKey))
			{
				MemberInfo[] memberInfo = enumType.GetMember(name);

				string description = name;
				if (memberInfo != null && memberInfo.Length > 0)
				{
					DescriptionAttribute attribute = memberInfo[0]
						.GetCustomAttributes(typeof(DescriptionAttribute), false)
						.Cast<DescriptionAttribute>().FirstOrDefault();
					if (attribute != null)
						description = attribute.Description;
				}

				CachedEnumDescriptions.Add(cacheKey, description);
			}
			return CachedEnumDescriptions[cacheKey];
		}

		public static IEnumerable<string> GetDescriptions(Type enumType)
		{
			string[] names = Enum.GetNames(enumType);
			return names.Select(s => GetEnumValueDescription(enumType, s));
		}

		public static bool TryParse<T>(string stringValue, out T value)
		{
			try
			{
				value = (T) Enum.Parse(typeof (T), stringValue);
				return true;
			}
			catch (ArgumentException)
			{
				value = default(T);
				return false;
			}
		}
	}
}