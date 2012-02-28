using Ext.Net;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes
{
	[ContentType("Tag Group", Description = "Defines a tag group that pages can be associated with.")]
	[RestrictParents(typeof(ITagGroupContainer))]
	public class TagGroup : BasePage
	{
		protected override Icon Icon
		{
			get { return Icon.TagBlue; }
		}
	}
}