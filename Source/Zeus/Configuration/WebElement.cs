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

		/// <summary>Look for a content page when the requested resource has no extension.</summary>
		[ConfigurationProperty("observeEmptyExtension")]
		public bool ObserveEmptyExtension
		{
			get { return (bool) base["observeEmptyExtension"] || string.IsNullOrEmpty(Extension) || Extension == "/"; }
			set { base["observeEmptyExtension"] = value; }
		}

		/// <summary>Additional extensions observed by the rewriter.</summary>
		[ConfigurationProperty("observedExtensions"), TypeConverter(typeof(CommaDelimitedStringCollectionConverter))]
		public StringCollection ObservedExtensions
		{
			get { return (CommaDelimitedStringCollection) base["observedExtensions"]; }
			set { base["observedExtensions"] = value; }
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