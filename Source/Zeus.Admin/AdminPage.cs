using System.Linq;
using Ext.Net;
using MongoDB.Bson;
using Zeus.Admin.Plugins.Tree;
using Zeus.Web;

namespace Zeus.Admin
{
	public abstract class AdminPage : System.Web.UI.Page
	{
		#region Fields

		private const string REFRESH_NAVIGATION_FORMAT = @"
jQuery(document).ready(function() {{
	if (window.top.zeus)
		window.top.zeus.refreshNavigation('{0}');
	else
		window.location = '{1}';
}});";
		private const string REFRESH_PREVIEW_FORMAT = @"
jQuery(document).ready(function() {{
	if (window.top.zeus)
		window.top.zeus.refreshPreview('{1}');
	else
		window.location = '{1}';
}});";
		private const string REFRESH_BOTH_FORMAT = @"
jQuery(document).ready(function() {{
	if (window.top.zeus)
		window.top.zeus.refresh('{0}', '{1}');
	else
		window.location = '{1}';
}});";

		#endregion

		#region Properties

		public virtual ContentItem SelectedItem
		{
			get
			{
				ContentItem selectedItem = GetFromViewState()
					?? GetFromUrl()
					?? Zeus.Context.UrlParser.StartPage;
				return selectedItem;
			}
			set
			{
				if (value != null)
					SelectedItemID = value.ID;
				else
					SelectedItemID = ObjectId.Empty;
			}
		}

		protected virtual INode SelectedNode
		{
			get { return SelectedItem; }
		}

		private ObjectId SelectedItemID
		{
			get { return (ObjectId)(ViewState["SelectedItemID"] ?? ObjectId.Empty); }
			set { ViewState["SelectedItemID"] = value; }
		}

		private ContentItem _memorizedItem;
		protected ContentItem MemorizedItem
		{
			get { return _memorizedItem ?? (_memorizedItem = Engine.Resolve<Navigator>().Navigate(Request.QueryString["memory"])); }
		}

		public virtual Engine.ContentEngine Engine
		{
			get { return Zeus.Context.Current; }
		}

		#endregion

		#region Methods

		protected virtual string CancelUrl()
		{
			return Request["returnUrl"] ?? SelectedNode.PreviewUrl;
		}

		public void Refresh(ContentItem contentItem, AdminFrame frame)
		{
			Refresh(contentItem, frame, GetPreviewUrl(contentItem));
		}

		public void Refresh(ContentItem contentItem, AdminFrame frame, string url)
		{
			string script = null;
			switch (frame)
			{
				case AdminFrame.Preview:
					script = REFRESH_PREVIEW_FORMAT;
					break;
				case AdminFrame.Navigation:
					script = REFRESH_NAVIGATION_FORMAT;
					break;
				case AdminFrame.Both:
					script = REFRESH_BOTH_FORMAT;
					break;
			}

			// If content item is not visible in tree, then get the first parent item
			// that is visible.
			contentItem = contentItem.AncestorsAndSelf
				.First(TreeMainInterfacePlugin.IsVisibleInTree);

			script = string.Format(script,
				contentItem.ID, // 0
				url // 1
			);

			if (ExtNet.IsAjaxRequest)
				ExtNet.ResourceManager.RegisterOnReadyScript(script);
			else
				ClientScript.RegisterStartupScript(
					typeof(AdminPage),
					"AddRefreshEditScript",
					script, true);
		}

		protected virtual string GetPreviewUrl(ContentItem selectedItem)
		{
			return Request["returnUrl"] ?? Engine.AdminManager.GetPreviewUrl(selectedItem);
		}

		private ContentItem GetFromViewState()
		{
			if (SelectedItemID != ObjectId.Empty)
				return ContentItem.Find(SelectedItemID);
			return null;
		}

		private ContentItem GetFromUrl()
		{
			string selected = GetSelectedPath();
			if (!string.IsNullOrEmpty(selected))
				return Engine.Resolve<Navigator>().Navigate(selected);

			string itemId = Request[PathData.ItemQueryKey];
			if (!string.IsNullOrEmpty(itemId))
				return ContentItem.Find(ObjectId.Parse(itemId));

			return null;
		}

		protected string GetSelectedPath()
		{
			return Request["selected"];
		}

		#endregion
	}
}
