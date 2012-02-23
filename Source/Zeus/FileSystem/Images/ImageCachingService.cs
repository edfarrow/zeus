using Ninject;
using Ormongo;
using SoundInTheory.DynamicImage.Caching;

namespace Zeus.FileSystem.Images
{
	public class ImageCachingService : IStartable
	{
		public void Start()
		{
			ContentItem.AfterDestroy += OnPersisterItemDeleted;
			ContentItem.AfterSave += OnPersisterItemSaved;
		}

		private void OnPersisterItemDeleted(object sender, DocumentEventArgs<ContentItem> e)
		{
			DeleteCachedImages(e.Document);
		}

		private void OnPersisterItemSaved(object sender, DocumentEventArgs<ContentItem> e)
		{
			DeleteCachedImages(e.Document);
		}

		public void DeleteCachedImages(ContentItem contentItem)
		{
			if (contentItem is Image)
			{
				var source = new OrmongoImageSource(((Image) contentItem).Data);
				DynamicImageCacheManager.Remove(source);
			}
		}

		public void Stop()
		{
			ContentItem.AfterDestroy -= OnPersisterItemDeleted;
			ContentItem.AfterSave -= OnPersisterItemSaved;
		}
	}
}