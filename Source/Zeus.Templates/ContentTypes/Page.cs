using Zeus.Design.Editors;
using Zeus.FileSystem;
using Zeus.FileSystem.Images;
using Zeus.Integrity;
using Zeus.Web;

namespace Zeus.Templates.ContentTypes
{
	[ContentType]
	[RestrictParents(typeof(WebsiteNode), typeof(Page), typeof(Redirect))]
	[AllowedChildren(typeof(File), typeof(Image))]
	public class Page : BasePage
	{
		[HtmlTextBoxEditor("Content", 30, ContainerName = "Content")]
		public virtual string Content { get; set; }
	}
}