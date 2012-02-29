using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;

namespace Zeus.Configuration
{
	/// <summary>
	/// Configuration related to integration with ASP.NET and urls.
	/// </summary>
	public class WebElement : ConfigurationElement
	{
		/// <summary>The default extension used by the url parser.</summary>
		[ConfigurationProperty("extension", DefaultValue = ".aspx")]
		public string Extension
		{
			get { return (string) base["extension"]; }
			set { base["extension"] = value; }
		}

		/// <summary>Tells the rewriter whether it should rewrite when the url matches an existing file. By default Zeus doesn't rewrite when the file exists.</summary>
		[ConfigurationProperty("ignoreExistingFiles", DefaultValue = false)]
		public bool IgnoreExistingFiles
		{
			get { return (bool) base["ignoreExistingFiles"]; }
			set { base["ignoreExistingFiles"] = value; }
		}
	}
}