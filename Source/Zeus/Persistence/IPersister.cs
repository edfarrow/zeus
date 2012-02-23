using System;
using MongoDB.Bson;

namespace Zeus.Persistence
{
	public interface IPersister
	{
		/// <summary>Occurs before an item is saved</summary>
		event EventHandler<CancelItemEventArgs> ItemSaving;
		/// <summary>Occurs when an item has been saved</summary>
		event EventHandler<ItemEventArgs> ItemSaved;
		/// <summary>Occurs before an item is copied</summary>
		event EventHandler<CancelDestinationEventArgs> ItemCopying;
		/// <summary>Occurs when an item has been copied</summary>
		event EventHandler<DestinationEventArgs> ItemCopied;
		/// <summary>Occurs when an item is loaded</summary>
		event EventHandler<ItemEventArgs> ItemLoaded;

		ContentItem Copy(ContentItem source, ContentItem destination);
		ContentItem Copy(ContentItem source, ContentItem destination, bool includeChildren);
		ContentItem Get(ObjectId id);
		T Get<T>(ObjectId id) where T : ContentItem;
		ContentItem Load(ObjectId id);
		void Save(ContentItem contentItem);
	}
}
