using Ext.Net;

namespace Zeus.Admin.Plugins
{
	public abstract class GridToolbarPluginBase : PluginBase, IGridToolbarPlugin
	{
		public abstract int SortOrder { get; }

		public virtual string[] RequiredScripts
		{
			get { return null; }
		}

		protected virtual string RequiredSecurityOperation
		{
			get { return null; }
		}

		public virtual string[] RequiredUserControls
		{
			get { return null; }
		}

		public virtual bool IsEnabled(ContentItem contentItem)
		{
			if (!string.IsNullOrEmpty(RequiredSecurityOperation))
			{
				// Check if user has permission to use this plugin.
				if (!Context.SecurityManager.IsAuthorized(contentItem, Context.Current.WebContext.User, RequiredSecurityOperation))
					return false;
			}

			return true;
		}

		public abstract Button GetToolbarButton(ContentItem contentItem, GridPanel gridPanel);
		public abstract void ModifyGrid(Button button, GridPanel gridPanel);
	}
}