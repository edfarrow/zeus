using System.Configuration;

namespace Zeus.Configuration
{
	public class DatabaseSection : ConfigurationSection
	{
		[ConfigurationProperty("serverHost", DefaultValue = "localhost")]
		public string ServerHost
		{
			get { return (string)base["serverHost"]; }
			set { base["serverHost"] = value; }
		}

		[ConfigurationProperty("serverPort", DefaultValue = 27017)]
		public int ServerPort
		{
			get { return (int)base["serverPort"]; }
			set { base["serverPort"] = value; }
		}

		[ConfigurationProperty("databaseName", DefaultValue = "Zeus")]
		public string DatabaseName
		{
			get { return (string)base["databaseName"]; }
			set { base["databaseName"] = value; }
		}
	}
}
