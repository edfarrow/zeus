using System;
using Zeus.EditableTypes;

namespace Zeus.Util
{
	internal static class EditableObjectUtility
	{
		public static object GetValue(IEditableObject item, string propertyName)
		{
			if (propertyName == null)
				throw new ArgumentNullException("propertyName");

			// If we have a class property matching this name, get the property value.
			// TODO: Cache this reflection
			var propertyInfo = item.GetType().GetProperty(propertyName);
			if (propertyInfo != null && propertyInfo.CanRead)
				return propertyInfo.GetValue(item, null);

			if (item.ExtraData.ContainsKey(propertyName))
				return item.ExtraData[propertyName];

			return null;
		}

		public static void SetValue(IEditableObject item, string propertyName, object value)
		{
			if (string.IsNullOrEmpty(propertyName))
				throw new ArgumentNullException("propertyName");

			// If we have a class property matching this name, set the property value.
			// TODO: Cache this reflection
			var propertyInfo = item.GetType().GetProperty(propertyName);
			if (propertyInfo != null && propertyInfo.CanWrite)
				propertyInfo.SetValue(item, value, null);
			else
				item.ExtraData[propertyName] = value;
		}
	}
}