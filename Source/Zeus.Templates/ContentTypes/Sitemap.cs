using Ext.Net;
using Zeus.Integrity;
using Zeus.Web;

namespace Zeus.Templates.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(WebsiteNode), typeof(Page))]
	public class Sitemap : BasePage
	{
		protected override Icon Icon
		{
			get { return Icon.Sitemap; }
		}
	}
}