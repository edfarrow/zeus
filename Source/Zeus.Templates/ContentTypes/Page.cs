using Zeus.Editors.Attributes;
using Zeus.Integrity;
using Zeus.Web;

namespace Zeus.Templates.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(WebsiteNode), typeof(Page), typeof(Redirect))]
	public class Page : BasePage
	{
		[HtmlTextBoxEditor("Content", 30, ContainerName = "Content")]
		public virtual string Content { get; set; }
	}
}