using Ext.Net;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(TagGroup))]
	public class Tag : BasePage
	{
		protected override Icon Icon
		{
			get { return Icon.TagRed; }
		}
	}
}