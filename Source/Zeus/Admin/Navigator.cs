using System;
using Zeus.Web;

namespace Zeus.Admin
{
	public class Navigator
	{
		private readonly IHost _host;

		public Navigator(IHost host)
		{
			_host = host;
		}

		public ContentItem Navigate(ContentItem startingPoint, string path)
		{
			return startingPoint.GetChild(path);
		}

		public ContentItem Navigate(string path)
		{
			if (path == null) throw new ArgumentNullException("path");
			if (!path.StartsWith("/"))
			{
				if (path.StartsWith("~"))
					return Navigate(ContentItem.FindOneByID(_host.CurrentSite.StartPageID), path.Substring(1));
				throw new ArgumentException("The path must start with a slash '/', was '" + path + "'", "path");
			}

			return Navigate(ContentItem.FindOneByID(_host.CurrentSite.RootItemID), path);
		}
	}
}
