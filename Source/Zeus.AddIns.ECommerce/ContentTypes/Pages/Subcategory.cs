using Ext.Net;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType]
	[RestrictParents(typeof(Category))]
	public class Subcategory : BasePage
	{
		protected override Icon Icon
		{
			get { return Icon.PageRed; }
		}
	}
}