using System;

namespace Zeus.BaseLibrary.ExtensionMethods
{
	public static class TypeExtensionMethods
	{
		public static bool IsNullable(this Type type)
		{
			return (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
		}

		/// <summary>
		/// If the type is nullable, returns the underlying type, otherwise returns
		/// the original type.
		/// </summary>
		/// <returns></returns>
		public static Type GetTypeOrUnderlyingType(this Type type)
		{
			return type.IsNullable() ? Nullable.GetUnderlyingType(type) : type;
		}
	}
}