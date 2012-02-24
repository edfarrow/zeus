using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using Zeus.BaseLibrary.Web;
using Zeus.Configuration;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Engine;
using Zeus.Linq;
using Zeus.Security;
using Zeus.Web.Hosting;

namespace Zeus.Admin
{
	/// <summary>
	/// Class responsible for plugins in edit mode, knowing links to edit 
	/// pages and saving interaction.
	/// </summary>
	public class AdminManager : IAdminManager
	{
		#region Fields

		private readonly AdminSection _configSection;
		private readonly ISecurityManager _securityManager;
		private readonly IAdminAssemblyManager _adminAssembly;
		private readonly IContentTypeManager _contentTypeManager;

		private readonly IEnumerable<ActionPluginGroupAttribute> _cachedActionPluginGroups;

		#endregion

		#region Constructor

		public AdminManager(AdminSection configSection, ISecurityManager securityManager, IAdminAssemblyManager adminAssembly,
			IContentTypeManager contentTypeManager,
			IPluginFinder<ActionPluginGroupAttribute> actionPluginGroupFinder,
			IEmbeddedResourceManager embeddedResourceManager)
		{
			_configSection = configSection;
			_securityManager = securityManager;
			_adminAssembly = adminAssembly;
			DeleteItemUrl = embeddedResourceManager.GetServerResourceUrl(adminAssembly.Assembly, "Zeus.Admin.Delete.aspx");
			EditItemUrl = embeddedResourceManager.GetServerResourceUrl(adminAssembly.Assembly, "Zeus.Admin.Plugins.EditItem.Default.aspx");
			NewItemUrl = embeddedResourceManager.GetServerResourceUrl(adminAssembly.Assembly, "Zeus.Admin.New.aspx");
			_contentTypeManager = contentTypeManager;

			_cachedActionPluginGroups = actionPluginGroupFinder.GetPlugins().OrderBy(g => g.SortOrder);
		}

		#endregion

		#region Properties

		public string DeleteItemUrl { get; set; }
		public string EditItemUrl { get; set; }
		public string NewItemUrl { get; set; }

		public bool TreeTooltipsEnabled
		{
			get { return (_configSection.Tree == null || _configSection.Tree.TooltipsEnabled); }
		}

		#endregion

		#region Methods

		public IEnumerable<ActionPluginGroupAttribute> GetActionPluginGroups()
		{
			return _cachedActionPluginGroups;
		}

		public string GetAdminDefaultUrl()
		{
			return Context.Current.GetServerResourceUrl(_adminAssembly.Assembly, "Zeus.Admin.Default.aspx");
		}

		public Func<IEnumerable<ContentItem>, IEnumerable<ContentItem>> GetEditorFilter(IPrincipal user)
		{
			return items => items.Authorized(user, _securityManager, Operations.Read).Pages();
		}

		/// <summary>Gets the url for the preview frame.</summary>
		/// <param name="selectedItem">The currently selected item.</param>
		/// <returns>An url.</returns>
		public string GetPreviewUrl(INode selectedItem)
		{
			string url = string.Format("{0}",
				selectedItem.PreviewUrl,
				HttpUtility.UrlEncode(selectedItem.PreviewUrl)
				);
			return Url.ToAbsolute(url);
		}

		/// <summary>Saves an item using values from the supplied item editor.</summary>
		/// <param name="item">The item to update.</param>
		/// <param name="addedEditors">The editors to update the item with.</param>
		/// <param name="user">The user that is performing the saving.</param>
		/// <param name="onSavingCallback"> </param>
		public virtual ContentItem Save(ContentItem item, IDictionary<string, Control> addedEditors, IPrincipal user,
			Action<ContentItem> onSavingCallback)
		{
			bool wasUpdated = UpdateItem(item, addedEditors, user);
			if (wasUpdated || item.IsNewRecord)
			{
				onSavingCallback(item);
				item.Save();

				ContentItem theParent = item.Parent;
				while (theParent.Parent != null) // TODO: Shouldn't this be theParent != null ?
				{
					//go up the tree updating - if a child has been changed, so effectively has the parent
					theParent.Save();
					theParent = theParent.Parent;
				}
			}

			return item;
		}

		/// <summary>Updates the item by way of letting the defined editable attributes interpret the added editors.</summary>
		/// <param name="item">The item to update.</param>
		/// <param name="addedEditors">The previously added editors.</param>
		/// <param name="user">The user for filtering updatable editors.</param>
		/// <returns>Whether any property on the item was updated.</returns>
		public bool UpdateItem(ContentItem item, IDictionary<string, Control> addedEditors, IPrincipal user)
		{
			if (item == null) throw new ArgumentNullException("item");
			if (addedEditors == null) throw new ArgumentNullException("addedEditors");

			bool updated = false;
			ContentType contentType = _contentTypeManager.GetContentType(item);
			foreach (IEditor e in contentType.GetEditors(user))
            {
				if (addedEditors.ContainsKey(e.Name))
                {
					updated = e.UpdateItem(item, addedEditors[e.Name]) || updated;
                    /*
                    if (updated)
                    {
                        System.Web.HttpContext.Current.Response.Write("Editor Name = " + e.Name + "<br/>");
                        System.Web.HttpContext.Current.Response.Write("Item Name = " + item.Name + "<br/>");
                        System.Web.HttpContext.Current.Response.Write("Item Url = " + item.Url + "<br/>");
                        System.Web.HttpContext.Current.Response.End();
                    }
                     */
                    
                }
            }
            

			return updated;
		}

		#endregion
	}
}
