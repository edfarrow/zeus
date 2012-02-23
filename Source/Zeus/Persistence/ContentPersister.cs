using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Zeus.Persistence
{
	public class ContentPersister : IPersister
	{
		#region Events

		/// <summary>Occurs before an item is saved</summary>
		public event EventHandler<CancelItemEventArgs> ItemSaving;

		/// <summary>Occurs when an item has been saved</summary>
		public event EventHandler<ItemEventArgs> ItemSaved;

		/// <summary>Occurs before an item is deleted</summary>
		public event EventHandler<CancelItemEventArgs> ItemDeleting;

		/// <summary>Occurs when an item has been deleted</summary>
		public event EventHandler<ItemEventArgs> ItemDeleted;

		/// <summary>Occurs before an item is copied</summary>
		public event EventHandler<CancelDestinationEventArgs> ItemCopying;

		/// <summary>Occurs when an item has been copied</summary>
		public event EventHandler<DestinationEventArgs> ItemCopied;

		/// <summary>Occurs when an item is loaded</summary>
		public event EventHandler<ItemEventArgs> ItemLoaded;

		#endregion

		#region Methods

		/// <summary>Copies an item and all sub-items to a destination</summary>
		/// <param name="source">The item to copy</param>
		/// <param name="destination">The destination below which to place the copied item</param>
		/// <returns>The copied item</returns>
		public virtual ContentItem Copy(ContentItem source, ContentItem destination)
		{
			return Copy(source, destination, true);
		}

		/// <summary>Copies an item and all sub-items to a destination</summary>
		/// <param name="source">The item to copy</param>
		/// <param name="destination">The destination below which to place the copied item</param>
		/// <param name="includeChildren">Whether child items should be copied as well.</param>
		/// <returns>The copied item</returns>
		public virtual ContentItem Copy(ContentItem source, ContentItem destination, bool includeChildren)
		{
			return Utility.InvokeEvent(ItemCopying, this, source, destination, (copiedItem, destinationItem) =>
			{
				ContentItem cloned = source.Clone();

				cloned.Parent = destination;
				Save(cloned);

				Invoke(ItemCopied, new DestinationEventArgs(cloned, destinationItem));

				return cloned;
			});
		}

		public void Delete(ContentItem itemNoMore)
		{
			Utility.InvokeEvent(ItemDeleting, itemNoMore, this, DeleteAction);
		}

		private void DeleteAction(ContentItem itemNoMore)
		{
			DeleteRecursive(itemNoMore);
			Invoke(ItemDeleted, new ItemEventArgs(itemNoMore));
		}

		private void DeleteRecursive(ContentItem contentItem)
		{
			List<ContentItem> children = new List<ContentItem>(contentItem.Children);
			foreach (ContentItem child in children)
				DeleteRecursive(child);

			contentItem.AddTo(null);

			DeleteInboundLinks(contentItem);

			contentItem.Destroy();
		}

		private void DeleteInboundLinks(ContentItem itemNoMore)
		{
			// TODO: Reimplement for Mongo
			//foreach (LinkProperty detail in _linkFinder.QueryDetails<LinkProperty>().Where(ld => ld.LinkedItem == itemNoMore))
			//{
			//    if (detail.EnclosingCollection != null)
			//        detail.EnclosingCollection.Remove(detail);
			//    IDictionary<string, PropertyData> test = detail.EnclosingItem.Details; // TODO: Investigate why this is necessary, on a PersistentGenericMap
			//    int count = test.Count;
			//    detail.EnclosingItem.Details.Remove(detail.Name);
			//    _linkRepository.Delete(detail);
			//}
		}

		public ContentItem Get(ObjectId id)
		{
			return ContentItem.FindOneByID(id);
		}

		public T Get<T>(ObjectId id)
			where T : ContentItem
		{
			return (T) ContentItem.FindOneByID(id);
		}

		public ContentItem Load(ObjectId id)
		{
			return ContentItem.FindOneByID(id);
		}

		public void Save(ContentItem unsavedItem)
		{
			Utility.InvokeEvent(ItemSaving, unsavedItem, this, SaveAction);
		}

		private void SaveAction(ContentItem contentItem)
		{
			contentItem.Updated = DateTime.Now;
			contentItem.Save();
			contentItem.AddTo(contentItem.Parent);

			Invoke(ItemSaved, new ItemEventArgs(contentItem));
		}

		protected virtual T Invoke<T>(EventHandler<T> handler, T args)
						where T : ItemEventArgs
		{
			if (handler != null)
				handler.Invoke(this, args);
			return args;
		}

		#endregion
	}
}
