using System;
using System.ComponentModel;
using System.Configuration;
using MongoDB.Bson;

namespace Zeus.Configuration
{
	public class CustomUrlsIDElement : ConfigurationElement
	{
		[ConfigurationProperty("id", IsRequired = true)]
		[TypeConverter(typeof(ObjectIdConverter))]
		public ObjectId ID
		{
			get { return (ObjectId)this["id"]; }
			set { this["id"] = value; }

		}

		[ConfigurationProperty("depth", IsRequired = true)]
		public int Depth
		{
			get { return (int) this["depth"]; }
			set { this["depth"] = value; }

		}
	}
}