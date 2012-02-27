using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Handlers;
using Ext.Net;
using Zeus.BaseLibrary.ExtensionMethods;
using Zeus.Web.Hosting;

namespace Zeus
{
	/// <summary>
	/// Mixed utility functions used by Zeus.
	/// </summary>
	public static class Utility
	{
		/// <summary>Converts a value to a destination type.</summary>
		/// <param name="value">The value to convert.</param>
		/// <param name="destinationType">The type to convert the value to.</param>
		/// <returns>The converted value.</returns>
		public static object Convert(object value, Type destinationType)
		{
			if (value != null)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(destinationType);
				if (converter != null && converter.CanConvertFrom(value.GetType()))
					return converter.ConvertFrom(value);
				converter = TypeDescriptor.GetConverter(value.GetType());
				if (converter != null && converter.CanConvertTo(destinationType))
					return converter.ConvertTo(value, destinationType);
				if (destinationType.IsEnum && value is int)
					return Enum.ToObject(destinationType, (int) value);
				if (!destinationType.IsAssignableFrom(value.GetType()))
				{
					if (!(value is IConvertible))
						throw new ZeusException("Cannot convert object of type '{0}' because it does not implement IConvertible", value.GetType());
					if (destinationType.IsNullable())
					{
						NullableConverter nullableConverter = new NullableConverter(destinationType);
						destinationType = nullableConverter.UnderlyingType;
					}
					return System.Convert.ChangeType(value, destinationType);
				}
			}
			return value;
		}

		/// <summary>Converts a value to a destination type.</summary>
		/// <param name="value">The value to convert.</param>
		/// <typeparam name="T">The type to convert the value to.</typeparam>
		/// <returns>The converted value.</returns>
		public static T Convert<T>(object value)
		{
			return (T) Convert(value, typeof(T));
		}

		public static Func<DateTime> CurrentTime = () => DateTime.Now;

		/// <summary>Gets a value from a property.</summary>
		/// <param name="instance">The object whose property to get.</param>
		/// <param name="propertyName">The name of the property to get.</param>
		/// <returns>The value of the property.</returns>
		public static object GetProperty(object instance, string propertyName)
		{
			if (instance == null) throw new ArgumentNullException("instance");
			if (propertyName == null) throw new ArgumentNullException("propertyName");

			Type instanceType = instance.GetType();
			PropertyInfo pi = instanceType.GetProperty(propertyName);

			if (pi == null)
				throw new ZeusException("No property '{0}' found on the instance of type '{1}'.", propertyName, instanceType);

			return pi.GetValue(instance, null);
		}

		public static string GetSafeName(string value)
		{
			return value.ToSafeUrl();
		}

		public static string GetUrlSafeString(string value)
		{
			value = value.ToLower().Replace(' ', '-');
			return Regex.Replace(value, @"[^a-zA-Z0-9\-]", string.Empty);
		}

		private static readonly ResourceManager _resourceManager = new ResourceManager();
		public static string GetCooliteIconUrl(Icon icon)
		{
			return _resourceManager.GetIconUrl(icon);
		}
	}
}
