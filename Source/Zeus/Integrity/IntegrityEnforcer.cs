using System;
using Ninject;
using Ormongo.Ancestry;
using Zeus.Persistence;

namespace Zeus.Integrity
{
	public class IntegrityEnforcer : ContentItemObserver, IIntegrityEnforcer, IStartable
	{
		private readonly IIntegrityManager _integrityManager;

		public IntegrityEnforcer(IIntegrityManager integrityManager)
		{
			_integrityManager = integrityManager;
		}

		public override bool BeforeCopy(ContentItem document, ContentItem newParent)
		{
			ZeusException ex = _integrityManager.GetCopyException(document, newParent);
			if (ex != null)
				throw ex;
			return base.BeforeCopy(document, newParent);
		}

		public override bool BeforeDestroy(ContentItem document)
		{
			ZeusException ex = _integrityManager.GetDeleteException(document);
			if (ex != null)
				throw ex;
			return base.BeforeDestroy(document);
		}

		public override void AfterDestroy(ContentItem document)
		{
			// TODO: Delete inbound links
			
			base.AfterDestroy(document);
		}

		public override bool BeforeMove(ContentItem document, ContentItem newParent)
		{
			ZeusException ex = _integrityManager.GetMoveException(document, newParent);
			if (ex != null)
				throw ex;
			return base.BeforeMove(document, newParent);
		}

		public override bool BeforeSave(ContentItem document)
		{
			ZeusException ex = _integrityManager.GetSaveException(document);
			if (ex != null)
				throw ex;
			return base.BeforeSave(document);
		}

		public void Start()
		{
			ContentItem.OrphanStrategy = OrphanStrategy.Destroy;
			ContentItem.Observers.Add(this);
		}

		public void Stop()
		{
			ContentItem.Observers.Remove(this);
		}
	}
}
