﻿using System;

namespace Zeus.Web
{
	/// <summary>
	/// Classes implementing this interface creates unique urls for items and
	/// can from such an url find the corresponding item.
	/// </summary>
	public interface IUrlParser
	{
		/// <summary>Is invoked when the url parser didn't find </summary>
		event EventHandler<PageNotFoundEventArgs> PageNotFound;

		/// <summary>Gets the current start page.</summary>
		ContentItem StartPage { get; }

		/// <summary>Parses the current url to retrieve the current page.</summary>
		ContentItem CurrentPage { get; }

		/// <summary>Calculates an item url by walking it's parent path.</summary>
		/// <param name="item">The item whose url to compute.</param>
		/// <returns>A friendly url to the supplied item.</returns>
		string BuildUrl(ContentItem item);

		/// <summary>Checks if an item is start or root page</summary>
		/// <param name="item">The item to check</param>
		/// <returns>True if the item is a start page or a root page</returns>
		bool IsRootOrStartPage(ContentItem item);

		/// <summary>Finds a page by traversing names from the start page.</summary>
		/// <param name="url">The url that should be traversed.</param>
		/// <returns>The content page matching the supplied url.</returns>
		ContentItem ParsePage(string url);
	}
}
