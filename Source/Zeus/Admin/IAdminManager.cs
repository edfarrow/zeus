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
		string AdminPath { get; }
		IEnumerable<ActionPluginGroupAttribute> GetActionPluginGroups();

		/// <summary>Gets the filter to be applied to items displayed in edit mode.</summary>
		/// <param name="user">The user for whom to apply the filter.</param>
		/// <returns>A filter.</returns>
		Func<IEnumerable<ContentItem>, IEnumerable<ContentItem>> GetEditorFilter(IPrincipal user);

		/// <summary>Gets the url for the preview frame.</summary>
		/// <param name="selectedItem">The currently selected item.</param>
		/// <returns>A url.</returns>
		string GetPreviewUrl(INode selectedItem);

		bool TreeTooltipsEnabled { get; }
	}
}
