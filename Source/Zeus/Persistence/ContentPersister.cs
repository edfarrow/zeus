using System;
using MongoDB.Bson;

namespace Zeus.Persistence
{
	public class ContentPersister : IPersister
	{
		#region Events

		/// <summary>Occurs before an item is copied</summary>
		public event EventHandler<CancelDestinationEventArgs> ItemCopying;

		/// <summary>Occurs when an item has been copied</summary>
		public event EventHandler<DestinationEventArgs> ItemCopied;

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
				cloned.Save();

				Invoke(ItemCopied, new DestinationEventArgs(cloned, destinationItem));

				return cloned;
			});
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
