using Zeus.BaseLibrary.Web;
using Zeus.Configuration;

namespace Zeus.Web
{
	/// <summary>
	/// Parses urls in a multiple host environment.
	/// </summary>
	public class MultipleSitesUrlParser : UrlParser
	{
		#region Constructors

        public MultipleSitesUrlParser(IWebContext webContext, IHost host, HostSection config, CustomUrlsSection urls)
			: base(host, webContext, config, urls)
		{

		}

		#endregion

		public override ContentItem Parse(string url)
		{
			if (url.StartsWith("/") || url.StartsWith("~/"))
				return base.Parse(url);

			Site site = Host.GetSite(url);
			if (site != null)
				return TryLoadingFromQueryString(url, PathData.ItemQueryKey, PathData.PageQueryKey)
					?? Parse(ContentItem.Find(site.StartPageID), Url.Parse(url).PathAndQuery);

			return TryLoadingFromQueryString(url, PathData.ItemQueryKey, PathData.PageQueryKey);
		}

		public override ContentItem StartPage
		{
			get { return GetStartPage(_webContext.Url); }
		}

		protected override ContentItem GetStartPage(Url url)
		{
			if (!url.IsAbsolute)
				return StartPage;
			Site site = Host.GetSite(url) ?? Host.CurrentSite;
			return ContentItem.Find(site.StartPageID);
		}

		public override string BuildUrl(ContentItem item)
		{
			ContentItem startPage;
			Url url = BuildUrlInternal(item, out startPage);

			if (startPage != null)
			{
				if (startPage.ID == Host.CurrentSite.StartPageID)
				{
					// the start page belongs to the current site, use relative url
					return url;
					//return Url.ToAbsolute("~" + url.PathAndQuery);
				}

				// find the start page and use it's host name
				foreach (Site site in Host.Sites)
					if (startPage.ID == site.StartPageID)
						return GetHostedUrl(item, url, site);
			}

			return url;
		}

		private static string GetHostedUrl(ContentItem item, string url, Site site)
		{
			string hostName = site.GetHostName();
			if (string.IsNullOrEmpty(hostName))
				return item.FindPath(PathData.DefaultAction).RewrittenUrl;

			return Url.Parse(url).SetAuthority(hostName);
		}

		protected override bool IsStartPage(ContentItem item)
		{
			foreach (Site site in Host.Sites)
				if (IsStartPage(item, site))
					return true;
			return base.IsStartPage(item);
		}
	}
}