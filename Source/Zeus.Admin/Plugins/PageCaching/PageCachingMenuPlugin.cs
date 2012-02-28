using Ext.Net;
using Zeus.Security;
using Zeus.Util;

namespace Zeus.Admin.Plugins.PageCaching
{
	[ActionPluginGroup("Caching", 40)]
	public class PageCachingMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		protected override bool AvailableByDefault
		{
			get { return false; }
		}

		public override string GroupName
		{
			get { return "Caching"; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.Administer; }
		}

		public override int SortOrder
		{
			get { return 1; }
		}

		public override bool IsApplicable(ContentItem contentItem)
		{
			if (!contentItem.IsPage)
				return false;

			return base.IsApplicable(contentItem);
		}

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format("function() {{ Ext.net.DirectMethods.PageCaching.ShowDialog('{0}', {{ url: '{1}' }}); }}",
				contentItem.ID, GetAdminDefaultUrl());
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Page Caching",
				IconUrl = IconUtility.GetIconUrl(Icon.Package),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}