using Ninject;
using Zeus.Persistence;

namespace Zeus.Admin.NavigationFlag
{
	public class NavigationCachingService : ContentItemObserver, IInitializable
	{
		public void Initialize()
		{
			ContentItem.Observers.Add(this);
		}

		public override void AfterDestroy(ContentItem document)
		{
			DeleteCachedNav(document);
			base.AfterDestroy(document);
		}

		public override void AfterSave(ContentItem document)
		{
			DeleteCachedNav(document);
			base.AfterSave(document);
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
	}
}