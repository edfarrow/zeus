using System.Collections.Generic;
using Zeus.Editors.Attributes;
using Zeus.Web;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
	public abstract class LegacyContentItem : PageContentItem
	{
		private readonly Dictionary<string, object> _values = new Dictionary<string,object>();
 
		public T GetDetail<T>(string name, T defaultValue)
		{
			if (!_values.ContainsKey(name))
				return defaultValue;
			return (T) _values[name];
		}

		public void SetDetail<T>(string name, T value)
		{
			_values[name] = value;
		}
	}

	[ContentType("Legacy Page")]
	public class LegacyPage : LegacyContentItem
	{
		[AutoEditor("Description", 100)]
		public string Description
		{
			get { return GetDetail<string>("Enquiry", null); }
			set { SetDetail("Enquiry", value); }
		}
	}
}