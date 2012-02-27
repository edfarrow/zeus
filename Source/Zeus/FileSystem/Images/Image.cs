using SoundInTheory.DynamicImage;
using SoundInTheory.DynamicImage.Caching;
using SoundInTheory.DynamicImage.Filters;
using SoundInTheory.DynamicImage.Layers;
using Zeus.Editors.Attributes;

namespace Zeus.FileSystem.Images
{
	[ContentType]
	public class Image : File
	{
		public Image()
		{
			base.Visible = false;
		}

		[EmbeddedImageEditor("Image", 100)]
		public override EmbeddedFile Data
		{
			get { return base.Data; }
			set { base.Data = value; }
		}

		public virtual string GetUrl(int width, int height, bool fill, DynamicImageFormat format)
		{
			Composition image = new Composition();
            image.ImageFormat = format;
            ImageLayer imageLayer = new ImageLayer();

            imageLayer.Source = new OrmongoImageSource(Data.Data);

            ResizeFilter resizeFilter = new ResizeFilter();
		    resizeFilter.Mode = fill ? ResizeMode.UniformFill : ResizeMode.Uniform;
		    resizeFilter.Width = Unit.Pixel(width);
		    resizeFilter.Height = Unit.Pixel(height);

            imageLayer.Filters.Add(resizeFilter);
            
		    image.Layers.Add(imageLayer);                

			return ImageUrlGenerator.GetImageUrl(image);

            /*old code replaced
             * 
            return new DynamicImageBuilder()
				.WithLayer(
					LayerBuilder.Image.SourceImage(this).WithFilter(FilterBuilder.Resize.To(width, height, fill)))
				.Url;
             */
		}

        public virtual string GetUrl(int width, int height, bool fill)
		{
            return GetUrl(width, height, fill, DynamicImageFormat.Jpeg);
		}

		public virtual string GetUrl(int width, int height)
		{
            return GetUrl(width, height, true, DynamicImageFormat.Jpeg);
		}
	}
}