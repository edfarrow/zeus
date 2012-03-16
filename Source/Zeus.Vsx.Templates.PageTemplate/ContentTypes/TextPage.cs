using Zeus;
using Zeus.Editors.Attributes;
using Zeus.Web;
using Zeus.Templates.ContentTypes;

namespace $rootnamespace$.ContentTypes
{
	[ContentType("$itemname$")]
	public class $safeitemname$ : BasePage
	{
		[AutoEditor("Text", 100)]
		public virtual string Text { get; set; }
	}
}
