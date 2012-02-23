using Ormongo.Ancestry;

namespace Zeus.Persistence
{
	public class ContentItemObserver : AncestryObserver<ContentItem>, IContentItemObserver
	{
		public virtual bool BeforeCopy(ContentItem document, ContentItem newParent)
		{
			return true;
		}

		public virtual void AfterCopy(ContentItem document, ContentItem newParent)
		{
			
		}
	}
}