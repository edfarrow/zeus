using System.Configuration;

namespace Zeus.Configuration
{
	public class ContentTypesSection : ConfigurationSection
	{
		[ConfigurationProperty("rules")]
		public ContentTypeRuleCollection Rules
		{
			get { return (ContentTypeRuleCollection)base["rules"]; }
			set { base["rules"] = value; }
		}
	}
}