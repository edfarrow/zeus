using System.Configuration;

namespace Zeus.Configuration
{
	public class HostNameElement : ConfigurationElement
	{
		[ConfigurationProperty("name", IsRequired = true)]
		public string Name
		{
			get { return (string) base["name"]; }
			set { base["name"] = value; }
		}
	}
}