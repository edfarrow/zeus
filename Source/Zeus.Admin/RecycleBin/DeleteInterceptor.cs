using Ninject;
using Zeus.Persistence;

namespace Zeus.Admin.RecycleBin
{
	/// <summary>
	/// Intercepts delete operations.
	/// </summary>
	public class DeleteInterceptor : ContentItemObserver, IStartable
	{
		private readonly IRecycleBinHandler _recycleBinHandler;

		public DeleteInterceptor(IRecycleBinHandler recycleBinHandler)
		{
			_recycleBinHandler = recycleBinHandler;
		}

		public void Start()
		{
			ContentItem.Observers.Add(this);
		}

		public void Stop()
		{
			ContentItem.Observers.Remove(this);
		}

		public override void AfterCopy(ContentItem document, ContentItem newParent)
		{
			if (LeavingTrash(document, newParent))
				_recycleBinHandler.RestoreValues(document);
			else if (_recycleBinHandler.IsInTrash(newParent))
				_recycleBinHandler.ExpireTrashedItem(document);
			base.AfterCopy(document, newParent);
		}


		public override bool BeforeMove(ContentItem document, ContentItem newParent)
		{
			if (LeavingTrash(document, newParent))
				_recycleBinHandler.RestoreValues(document);
			else if (_recycleBinHandler.IsInTrash(newParent))
				_recycleBinHandler.ExpireTrashedItem(document);
			return base.BeforeMove(document, newParent);
		}

		public override bool BeforeDestroy(ContentItem document)
		{
			if (_recycleBinHandler.CanThrow(document))
			{
				_recycleBinHandler.Throw(document);
				return false;
			}
			return base.BeforeDestroy(document);
		}

		private bool LeavingTrash(ContentItem item, ContentItem newParent)
		{
			return item["DeletedDate"] != null && !_recycleBinHandler.IsInTrash(newParent);
		}
	}
}