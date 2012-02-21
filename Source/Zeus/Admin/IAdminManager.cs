using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web.UI;
using Zeus.ContentTypes;

namespace Zeus.Admin
{
	/// <summary>
	/// Classes implementing this interface can be used to interact with the 
	/// edit mode functionality.
	/// </summary>
	public interface IAdminManager
	{
		IEnumerable<ActionPluginGroupAttribute> GetActionPluginGroups();

		string GetAdminDefaultUrl();

		/// <summary>Gets the url to the edit page where to edit an existing item.</summary>
		/// <param name="item">The item to edit.</param>
		/// <returns>The url to the edit page</returns>
		string GetEditExistingItemUrl(ContentItem item);

		/// <summary>Gets the url to edit page creating new items.</summary>
		/// <param name="selected">The selected item.</param>
		/// <param name="contentType">The type of item to edit.</param>
		/// <returns>The url to the edit page.</returns>
		string GetEditNewPageUrl(ContentItem selected, ContentType contentType);

		/// <summary>Gets the url to the select type of item to create.</summary>
		/// <param name="selectedItem">The currently selected item.</param>
		/// <returns>The url to the select new item to create page.</returns>
		string GetSelectNewItemUrl(ContentItem selectedItem);

		/// <summary>Gets the filter to be applied to items displayed in edit mode.</summary>
		/// <param name="user">The user for whom to apply the filter.</param>
		/// <returns>A filter.</returns>
		Func<IEnumerable<ContentItem>, IEnumerable<ContentItem>> GetEditorFilter(IPrincipal user);

		/// <summary>Gets the url for the preview frame.</summary>
		/// <param name="selectedItem">The currently selected item.</param>
		/// <returns>A url.</returns>
		string GetPreviewUrl(INode selectedItem);

		/// <summary>Saves an item using values from the supplied item editor.</summary>
		/// <param name="item">The item to update.</param>
		/// <param name="addedEditors">The editors to update the item with.</param>
		/// <param name="user">The user that is performing the saving.</param>
		/// <param name="onSavingCallback"> </param>
		ContentItem Save(ContentItem item, IDictionary<string, Control> addedEditors, IPrincipal user, Action<ContentItem> onSavingCallback);

		/// <summary>Updates the item with the values from the editors.</summary>
		/// <param name="item">The item to update.</param>
		/// <param name="addedEditors">The previously added editors.</param>
		/// <param name="user">The user for filtering updatable editors.</param>
		bool UpdateItem(ContentItem item, IDictionary<string, Control> addedEditors, IPrincipal user);

		bool TreeTooltipsEnabled { get; }
	}
}
