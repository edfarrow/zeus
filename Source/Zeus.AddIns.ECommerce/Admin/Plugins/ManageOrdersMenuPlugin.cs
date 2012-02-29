using Ext.Net;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.Admin;
using Zeus.Admin.Plugins;
using Zeus.Util;

namespace Zeus.AddIns.ECommerce.Admin.Plugins
{
	[ActionPluginGroup("ECommerce", 100)]
	public class ManageOrdersMenuPlugin : MenuPluginBase, IContextMenuPlugin
	{
		public override string GroupName
		{
			get { return "ECommerce"; }
		}

		public override int SortOrder
		{
			get { return 1; }
		}

		public override bool IsApplicable(ContentItem contentItem)
		{
			// Hide if this is not the OrderContainer node
			if (!(contentItem is OrderContainer))
				return false;

			return base.IsApplicable(contentItem);
		}

		public override bool IsDefault(ContentItem contentItem)
		{
			return true;
		}

		public string GetJavascriptHandler(ContentItem contentItem)
		{
			return string.Format("function() {{ zeus.reloadContentPanel('Manage Orders', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.AddIns.ECommerce.Admin.Plugins.ManageOrders.Default.aspx") + "?selected=" + contentItem.Path);
		}

		public MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Manage Orders",
				IconUrl = IconUtility.GetIconUrl(Icon.Basket),
				Handler = GetJavascriptHandler(contentItem)
			};

			return menuItem;
		}
	}
}