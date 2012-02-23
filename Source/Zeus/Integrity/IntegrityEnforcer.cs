using System;
using Ninject;
using Ormongo;
using Ormongo.Ancestry;
using Zeus.Persistence;

namespace Zeus.Integrity
{
	public class IntegrityEnforcer : IIntegrityEnforcer, IStartable
	{
		private readonly IPersister _persister;
		private readonly IIntegrityManager _integrityManager;

		public IntegrityEnforcer(IPersister persister, IIntegrityManager integrityManager)
		{
			ContentItem.OrphanStrategy = OrphanStrategy.Destroy;
			_persister = persister;
			_integrityManager = integrityManager;
		}

		#region Event Dispatchers

		private void ItemSavingEventHandler(object sender, CancelItemEventArgs e)
		{
			OnItemSaving(e.AffectedItem);
		}

		private void ItemMovingEventHandler(object sender, CancelMoveDocumentEventArgs<ContentItem> e)
		{
			OnItemMoving(e.Document, e.NewParent);
		}

		private void ItemDeletingEventHandler(object sender, CancelDocumentEventArgs<ContentItem> e)
		{
			OnItemDeleting(e.Document);
		}

		private void ItemDestroyedEventHandler(object sender, DocumentEventArgs<ContentItem> e)
		{
			OnItemDestroyed(e.Document);
		}

		private void ItemCopyingEventHandler(object sender, CancelDestinationEventArgs e)
		{
			OnItemCopying(e.AffectedItem, e.Destination);
		}

		#endregion

		#region Event Handlers

		protected virtual void OnItemCopying(ContentItem source, ContentItem destination)
		{
			ZeusException ex = _integrityManager.GetCopyException(source, destination);
			if (ex != null)
				throw ex;
		}

		protected virtual void OnItemDeleting(ContentItem item)
		{
			ZeusException ex = _integrityManager.GetDeleteException(item);
			if (ex != null)
				throw ex;
		}

		protected virtual void OnItemDestroyed(ContentItem item)
		{
			// TODO: Delete inbound links
			throw new NotImplementedException();
		}

		protected virtual void OnItemMoving(ContentItem source, ContentItem destination)
		{
			ZeusException ex = _integrityManager.GetMoveException(source, destination);
			if (ex != null)
				throw ex;
		}

		protected virtual void OnItemSaving(ContentItem item)
		{
			ZeusException ex = _integrityManager.GetSaveException(item);
			if (ex != null)
				throw ex;
		}

		#endregion

		#region IStartable Members

		public virtual void Start()
		{
			ContentItem.BeforeDestroy += ItemDeletingEventHandler;
			ContentItem.AfterDestroy += ItemDestroyedEventHandler;
			_persister.ItemCopying += ItemCopyingEventHandler;
			ContentItem.BeforeMove += ItemMovingEventHandler;
			_persister.ItemSaving += ItemSavingEventHandler;
		}

		public virtual void Stop()
		{
			ContentItem.BeforeDestroy -= ItemDeletingEventHandler;
			ContentItem.AfterDestroy -= ItemDestroyedEventHandler;
			_persister.ItemCopying -= ItemCopyingEventHandler;
			ContentItem.BeforeMove -= ItemMovingEventHandler;
			_persister.ItemSaving -= ItemSavingEventHandler;
		}

		#endregion
	}
}
