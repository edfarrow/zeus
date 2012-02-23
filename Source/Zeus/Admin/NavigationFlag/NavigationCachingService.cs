using Ninject;
using Ormongo;

namespace Zeus.Admin.NavigationFlag
{
	public class NavigationCachingService : IStartable
	{
		public void Start()
		{
			ContentItem.AfterDestroy += OnPersisterItemDeleted;
			ContentItem.AfterSave += OnPersisterItemSaved;
		}

		private void OnPersisterItemDeleted(object sender, DocumentEventArgs<ContentItem> e)
		{
			DeleteCachedNav(e.Document);
		}

		private void OnPersisterItemSaved(object sender, DocumentEventArgs<ContentItem> e)
		{
			DeleteCachedNav(e.Document);
		}

		public void DeleteCachedNav(ContentItem contentItem)
		{
            //any time anything is saved or changed, delete all the primary nav app cache data
            if (contentItem.IsPage)
            {
                foreach (string item in System.Web.HttpContext.Current.Application.AllKeys)
                {
                    if (item.StartsWith("primaryNav"))
                    {
                        System.Web.HttpContext.Current.Application.Remove(item);
                    }
                }
            }
		}

		public void Stop()
		{
			ContentItem.AfterDestroy -= OnPersisterItemDeleted;
			ContentItem.AfterSave -= OnPersisterItemSaved;
		}
	}
}