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
		/// <summary>Occurs before an item is deleted</summary>
		event EventHandler<CancelItemEventArgs> ItemDeleting;
		/// <summary>Occurs when an item has been deleted</summary>
		event EventHandler<ItemEventArgs> ItemDeleted;
		/// <summary>Occurs before an item is moved</summary>
		event EventHandler<CancelDestinationEventArgs> ItemMoving;
		/// <summary>Occurs when an item has been moved</summary>
		event EventHandler<DestinationEventArgs> ItemMoved;
		/// <summary>Occurs before an item is copied</summary>
		event EventHandler<CancelDestinationEventArgs> ItemCopying;
		/// <summary>Occurs when an item has been copied</summary>
		event EventHandler<DestinationEventArgs> ItemCopied;
		/// <summary>Occurs when an item is loaded</summary>
		event EventHandler<ItemEventArgs> ItemLoaded;

		ContentItem Copy(ContentItem source, ContentItem destination);
		ContentItem Copy(ContentItem source, ContentItem destination, bool includeChildren);
		void Delete(ContentItem contentItem);
		ContentItem Get(ObjectId id);
		T Get<T>(ObjectId id) where T : ContentItem;
		ContentItem Load(ObjectId id);
		void Move(ContentItem toMove, ContentItem newParent);
		void Save(ContentItem contentItem);
		void UpdateSortOrder(ContentItem contentItem, int newPos);
	}
}
