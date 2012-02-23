using Ninject;
using Ormongo;
using SoundInTheory.DynamicImage.Caching;
using Zeus.Persistence;

namespace Zeus.FileSystem.Images
{
	public class ImageCachingService : IStartable
	{
		private readonly IPersister _persister;

		public ImageCachingService(IPersister persister)
		{
			_persister = persister;
		}

		public void Start()
		{
			ContentItem.AfterDestroy += OnPersisterItemDeleted;
			_persister.ItemSaved += OnPersisterItemSaved;
		}

		private void OnPersisterItemDeleted(object sender, DocumentEventArgs<ContentItem> e)
		{
			DeleteCachedImages(e.Document);
		}

		private void OnPersisterItemSaved(object sender, ItemEventArgs e)
		{
			DeleteCachedImages(e.AffectedItem);
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
			_persister.ItemSaved -= OnPersisterItemSaved;
		}
	}
}