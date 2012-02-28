using System.Web.Mvc;

namespace Zeus.Web.Mvc.Html
{
	public interface ITemplateRenderer
	{
		string RenderTemplate(HtmlHelper htmlHelper, ContentItem item, IContentItemContainer container, string action);
	}
}