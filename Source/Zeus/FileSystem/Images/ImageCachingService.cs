using Ninject;
using SoundInTheory.DynamicImage.Caching;
using Zeus.Persistence;

namespace Zeus.FileSystem.Images
{
	public class ImageCachingService : ContentItemObserver, IInitializable
	{
		public void Initialize()
		{
			ContentItem.Observers.Add(this);
		}

		public override void AfterDestroy(ContentItem document)
		{
			DeleteCachedImages(document);
			base.AfterDestroy(document);
		}

		public override void AfterSave(ContentItem document)
		{
			DeleteCachedImages(document);
			base.AfterSave(document);
		}

		public void DeleteCachedImages(ContentItem contentItem)
		{
			if (contentItem is Image)
			{
				var source = new OrmongoImageSource(((Image) contentItem).Data.Data);
				DynamicImageCacheManager.Remove(source);
			}
		}
	}
}