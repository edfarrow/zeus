using Zeus.Web;
using Zeus.Integrity;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
	[ContentType("Custom Url Page Container")]
    [RestrictParents(typeof(WebsiteNode))]
	public class CustomUrlContainer : PageContentItem
	{
	}
}
