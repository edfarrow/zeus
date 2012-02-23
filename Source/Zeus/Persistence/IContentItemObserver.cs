using Ormongo.Ancestry;

namespace Zeus.Persistence
{
	public interface IContentItemObserver : IAncestryObserver<ContentItem>
	{
		bool BeforeCopy(ContentItem document, ContentItem newParent);
		void AfterCopy(ContentItem document, ContentItem newParent);
	}
}