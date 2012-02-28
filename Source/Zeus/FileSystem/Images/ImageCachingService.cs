using Ninject;
using Ormongo;
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
			var image = contentItem as Image;
			if (image != null)
				DeleteCachedImages((image).Data.Data);
		}

		public void DeleteCachedImages(Attachment attachment)
		{
			var source = new OrmongoImageSource(attachment);
			DynamicImageCacheManager.Remove(source);
		}
	}
}