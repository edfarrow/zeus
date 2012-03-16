using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Web;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
	[ContentType("My Shop", IgnoreSEOAssets = true)]
	[RestrictParents(typeof(WebsiteNode))]
	public class MyShop : BasePage
	{
		protected override Ext.Net.Icon Icon
		{
			get { return Ext.Net.Icon.Money; }
		}
	}
}