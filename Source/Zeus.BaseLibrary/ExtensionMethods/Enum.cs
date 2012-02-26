using System;

namespace Zeus.BaseLibrary.ExtensionMethods
{
	public static class EnumExtensionMethods
	{
		public static string GetDescription(this Enum value)
		{
			return EnumHelper.GetEnumValueDescription(value.GetType(), value.ToString());
		}
	}
}