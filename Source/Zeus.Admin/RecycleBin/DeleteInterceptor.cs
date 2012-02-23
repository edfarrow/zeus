using Ninject;
using Ormongo.Ancestry;
using Zeus.Persistence;

namespace Zeus.Admin.RecycleBin
{
	/// <summary>
	/// Intercepts delete operations.
	/// </summary>
	public class DeleteInterceptor : IStartable
	{
		private readonly IPersister _persister;
		private readonly IRecycleBinHandler _recycleBinHandler;

		public DeleteInterceptor(IPersister persister, IRecycleBinHandler recycleBinHandler)
		{
			_persister = persister;
			_recycleBinHandler = recycleBinHandler;
		}

		public void Start()
		{
			_persister.ItemDeleting += OnItemDeleting;
			ContentItem.BeforeMove += OnItemMoving;
			_persister.ItemCopied += OnItemCopied;
		}

		public void Stop()
		{
			_persister.ItemDeleting -= OnItemDeleting;
			ContentItem.BeforeMove -= OnItemMoving;
			_persister.ItemCopied -= OnItemCopied;
		}

		private void OnItemCopied(object sender, DestinationEventArgs e)
		{
			if (LeavingTrash(e.AffectedItem, e.Destination))
				_recycleBinHandler.RestoreValues(e.AffectedItem);
			else if (_recycleBinHandler.IsInTrash(e.Destination))
				_recycleBinHandler.ExpireTrashedItem(e.AffectedItem);
		}

		private void OnItemMoving(object sender, CancelMoveDocumentEventArgs<ContentItem> e)
		{
			if (LeavingTrash(e.Document, e.NewParent))
				_recycleBinHandler.RestoreValues(e.Document);
			else if (_recycleBinHandler.IsInTrash(e.NewParent))
				_recycleBinHandler.ExpireTrashedItem(e.Document);
		}

		private void OnItemDeleting(object sender, CancelItemEventArgs e)
		{
			if (_recycleBinHandler.CanThrow(e.AffectedItem))
				e.FinalAction = _recycleBinHandler.Throw;
		}

		private bool LeavingTrash(ContentItem item, ContentItem newParent)
		{
			return item["DeletedDate"] != null && !_recycleBinHandler.IsInTrash(newParent);
		}
	}
}