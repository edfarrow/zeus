using Ormongo;
using SoundInTheory.DynamicImage.Fluent;

namespace Zeus.FileSystem.Images
{
	public static class ImageLayerBuilderExtensions
	{
		public static ImageLayerBuilder SourceImage(this ImageLayerBuilder builder, Image image)
		{
			builder.Source = new OrmongoImageSource(image.Data);
			return builder;
		}

		public static ImageLayerBuilder SourceImage(this ImageLayerBuilder builder, Attachment attachment)
		{
			builder.Source = new OrmongoImageSource(attachment);
			return builder;
		}
	}
}