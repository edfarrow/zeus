using Coolite.Ext.Web;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.Admin;

namespace Zeus.AddIns.ECommerce.ActionPlugins
{
	public class ManageOrdersActionPlugin : ActionPluginBase
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

		public override MenuItem GetMenuItem(ContentItem contentItem)
		{
			MenuItem menuItem = new MenuItem
			{
				Text = "Manage Orders",
				IconUrl = Utility.GetCooliteIconUrl(Icon.Basket)
			};

			menuItem.Handler = string.Format("function() {{ zeus.reloadContentPanel('Manage Orders', '{0}'); }}",
				GetPageUrl(GetType(), "Zeus.AddIns.ECommerce.Plugins.ManageOrders.aspx") + "?selected=" + contentItem.Path);

			return menuItem;
		}
	}
}