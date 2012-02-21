using System;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using MongoDB.Bson;

namespace Zeus.Configuration
{
	public class ObjectIdConverter : ConfigurationConverterBase
	{
		public override bool CanConvertTo(ITypeDescriptorContext ctx, Type type)
		{
			return type == typeof(string);
		}

		public override bool CanConvertFrom(ITypeDescriptorContext ctx, Type type)
		{
			return type == typeof(string);
		}

		public override object ConvertTo(ITypeDescriptorContext ctx, CultureInfo ci,
			object value, Type type)
		{
			return ((ObjectId) value).ToString();
		}

		public override object ConvertFrom(ITypeDescriptorContext ctx, CultureInfo ci, object data)
		{
			return ObjectId.Parse((string) data);
		}
	}
}