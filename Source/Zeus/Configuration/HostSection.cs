using System.ComponentModel;
using System.Configuration;
using MongoDB.Bson;

namespace Zeus.Configuration
{
	public class HostSection : ConfigurationSection
	{
		[ConfigurationProperty("rootItemID")]
		[TypeConverter(typeof(ObjectIdConverter))]
		public ObjectId RootItemID
		{
			get { return (ObjectId)base["rootItemID"]; }
			set { base["rootItemID"] = value; }
		}

		/// <summary>Examine content nodes to find items that are site providers.</summary>
		[ConfigurationProperty("dynamicSites", DefaultValue = true)]
		public bool DynamicSites
		{
			get { return (bool) base["dynamicSites"]; }
			set { base["dynamicSites"] = value; }
		}

		/// <summary>Use wildcard matching for hostnames, e.g. both sitdap.com and www.sitdap.com.</summary>
		[ConfigurationProperty("wildcards", DefaultValue = false)]
		public bool Wildcards
		{
			get { return (bool) base["wildcards"]; }
			set { base["wildcards"] = value; }
		}

		[ConfigurationProperty("sites")]
		public SiteElementCollection Sites
		{
			get { return (SiteElementCollection) base["sites"]; }
			set { base["sites"] = value; }
		}

		[ConfigurationProperty("web")]
		public WebElement Web
		{
			get { return (WebElement) base["web"]; }
			set { base["web"] = value; }
		}
	}
}
