using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Zeus.FileSystem;
using Zeus.Linq;
using Zeus.Templates.ContentTypes;
using Zeus.Web;
using Zeus.Web.Mvc.ViewModels;
using Zeus.Web.UI.WebControls;

namespace Zeus.Templates.Mvc.Html
{
	public static class NavigationExtensions
	{
		public delegate string CssClassFunc(ContentItem contentItem, bool isFirst, bool isLast);

		public static IEnumerable<ContentItem> NavigationItems(this HtmlHelper html)
		{
			return NavigationItems(html, Find.StartPage);
		}

		public static IEnumerable<ContentItem> NavigationItems(this HtmlHelper html, ContentItem startPage)
		{
			return startPage.GetGlobalizedChildren().Navigable();
		}

		public static string NavigationLinks(this HtmlHelper html, ContentItem startItem, Func<string, string> layoutCallback,
			CssClassFunc cssClassCallback, Func<ContentItem, string, string> activeTextCallback)
		{
			var navigationItems = NavigationItems(html, startItem);

			string result = string.Empty;
			foreach (ContentItem contentItem in navigationItems)
			{
				result += string.Format("<li class=\"{0}\"><span><a href=\"{1}\">{2}</a></span></li>",
					cssClassCallback(contentItem, contentItem == navigationItems.First(), contentItem == navigationItems.Last()),
					contentItem.Url, activeTextCallback(contentItem, contentItem.Title));
			}

			result = layoutCallback(result);
			return result;
		}

		public static string NavigationLinks(this HtmlHelper html, ContentItem startItem, ContentItem currentPage, string listClientId, string currentCssClass,
			Func<string, string> activeTextCallback)
		{
			return NavigationLinks(html,
				startItem,
				nl => "<ul id=\"" + listClientId + "\">" + nl + "</ul>",
				(ci, isFirst, isLast) =>
				{
					string result = string.Empty;
					if (IsCurrentBranch(html, ci, currentPage))
						result += currentCssClass;
					if (isLast)
						result += " last";
					return result;
				},
				(ci, title) =>
				{
					if (IsCurrentBranch(html, ci, currentPage))
						return activeTextCallback(title);
					return title;
				});
		}

		public static string NavigationLinks(this HtmlHelper html, ContentItem currentPage, string currentCssClass, Func<string, string> activeTextCallback)
		{
			return NavigationLinks(html, Find.StartPage, currentPage, null, currentCssClass, activeTextCallback);
		}

		public static string NavigationLinks(this HtmlHelper html, ContentItem currentPage)
		{
			return NavigationLinks(html, Find.StartPage, currentPage, null, "on", title => title);
		}

		/// <summary>
		/// Returns true if this page, or one of its descendents, is the current page.
		/// </summary>
		/// <param name="helper"></param>
		/// <param name="itemToCheck"></param>
		/// <returns></returns>
		public static bool IsCurrentBranch(this HtmlHelper helper, ContentItem itemToCheck)
		{
			return IsCurrentBranch(helper, itemToCheck, Find.CurrentPage);
		}

		public static bool IsCurrentBranch(this HtmlHelper helper, ContentItem itemToCheck, ContentItem currentPage)
		{
			if ((itemToCheck is Redirect))
			{
				Redirect redirect = (Redirect) itemToCheck;
				if (redirect.RedirectItem == currentPage)
					return true;
				if (redirect.CheckChildrenForNavigationState && Find.IsAccessibleChildOrSelf(((Redirect)itemToCheck).RedirectItem, currentPage))
					return true;
			}
			if (Find.IsAccessibleChildOrSelf(itemToCheck, currentPage))
				return true;
			return false;
		}

		public static string Breadcrumbs(this HtmlHelper html, ContentItem currentPage)
		{
			return Breadcrumbs(html, currentPage, "<ul id=\"crumbs\">", "</ul>", 1, 2, string.Empty,
				l => string.Format("<li><a href=\"{0}\">{1}</a></li>", l.Url, l.Contents),
				l => string.Format("<li class=\"last\">{0}</li>", l.Contents));
		}

		public static string Breadcrumbs(this HtmlHelper html, ContentItem currentPage, string prefix, string postfix, int startLevel, int visibilityLevel, string separatorText,
			Func<ILink, string> itemCallback, Func<ILink, string> lastItemCallback)
		{
			string result = postfix;

			int added = 0;
			var parents = Find.EnumerateParents(currentPage, Find.StartPage, true);
			if (startLevel != 1 && parents.Count() >= startLevel)
				parents = parents.Take(parents.Count() - startLevel);
			foreach (ContentItem page in parents)
			{
				IBreadcrumbAppearance appearance = page as IBreadcrumbAppearance;
				bool visible = appearance == null || appearance.VisibleInBreadcrumb;
				if (visible && page.IsPage)
				{
					ILink link = appearance ?? (ILink)page;
					if (added > 0)
					{
						result = separatorText + Environment.NewLine + result;
						result = GetBreadcrumbItem(link, itemCallback) + result;
					}
					else
					{
						result = GetBreadcrumbItem(link, lastItemCallback) + result;
					}
					result = Environment.NewLine + result;
					++added;
				}
			}

			result = prefix + result;

			if (added < visibilityLevel)
				result = string.Empty;

			return result;
		}

		private static string GetBreadcrumbItem(ILink link, Func<ILink, string> formatCallback)
		{
			return formatCallback(link);
		}

		public static string Sitemap(this HtmlHelper html)
		{
			StringBuilder sb = new StringBuilder();
			foreach (ContentItem contentItem in Find.StartPage.GetChildren().Pages().Visible())
			{
				sb.AppendFormat("<h4><a href=\"{0}\">{1}</a></h4>", contentItem.Url, contentItem.Title);
				SitemapRecursive(contentItem, sb);
			}
			return sb.ToString();
		}

		private static void SitemapRecursive(ContentItem contentItem, StringBuilder sb)
		{
			var childItems = contentItem.GetChildren().Pages().Visible();
			if (childItems.Any())
			{
				sb.Append("<ul>");
				foreach (ContentItem childItem in childItems)
				{
					sb.Append("<li>");
					if (childItem.Visible)
						sb.AppendFormat("<a href=\"{0}\">{1}</a>", childItem.Url, childItem.Title);
					else
						sb.Append(childItem.Title);
					SitemapRecursive(childItem, sb);
					sb.Append("</li>");
				}
				sb.Append("</ul>");
			}
		}
	}
}