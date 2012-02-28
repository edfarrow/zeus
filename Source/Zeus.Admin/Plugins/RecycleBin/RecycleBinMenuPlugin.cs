using Ext.Net;
using Zeus.Security;
using Zeus.Util;

namespace Zeus.Admin.Plugins.RecycleBin
{
	public class RecycleBinMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		public override string GroupName
		{
			get { return "Preview"; }
		}

		protected override string RequiredSecurityOperation
		{
			get { return Operations.Read; }
		}

		public override int SortOrder
		{
			get { return 1; }
		}

		public override bool IsApplicable(ContentItem contentItem)
		{
			if (!(contentItem is RecycleBinContainer))
				return false;

			return base.IsApplicable(contentItem);
		}

		public override bool IsDefault(ContentItem contentItem)
		{
			return true;
		}

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format("function() {{ top.zeus.reloadContentPanel('Recycle Bin', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.Admin.Plugins.RecycleBin.Default.aspx") + "?selected=" + contentItem.Path);
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "View Recycle Bin",
				IconUrl = IconUtility.GetIconUrl(Icon.Bin),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}