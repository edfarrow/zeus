using SoundInTheory.DynamicImage;
using SoundInTheory.DynamicImage.Caching;
using SoundInTheory.DynamicImage.Filters;
using SoundInTheory.DynamicImage.Layers;

namespace Zeus.FileSystem.Images
{
	public class EmbeddedCroppedImage : EmbeddedFile
	{
		#region JCrop values

		public int TopLeftXVal { get; set; }
		public int TopLeftYVal { get; set; }
		public int CropWidth { get; set; }
		public int CropHeight { get; set; }

		#endregion

        public string GetUrl(int width, int height, DynamicImageFormat format)
        {
            return GetUrl(width, height, format, false);
        }

        public string GetUrl(int width, int height, DynamicImageFormat format, bool isResize)
        {
            //first construct the crop
            var imageSource = new OrmongoImageSource(Data);

            // generate resized image url
            // set image format
            var dynamicImage = new Composition();
            dynamicImage.ImageFormat = format;
            
            // create image layer wit ha source
            var imageLayer = new ImageLayer();
            imageLayer.Source = imageSource;

            // add filters
            if (!(TopLeftXVal == 0 && TopLeftYVal == 0 && CropWidth == 0 && CropHeight == 0))
            {
                var cropFilter = new CropFilter
                {
                    Enabled = true,
                    X = TopLeftXVal,
                    Y = TopLeftYVal,
                    Width = CropWidth,
                    Height = CropHeight
                };
                if (!isResize)
                    imageLayer.Filters.Add(cropFilter);
            }

            if (width > 0 && height > 0)
            {
                var resizeFilter = new ResizeFilter
                {
                    Mode = isResize ? ResizeMode.Uniform : ResizeMode.UniformFill,
                    Width = Unit.Pixel(width),
                    Height = Unit.Pixel(height)
                };
                imageLayer.Filters.Add(resizeFilter);
            }
            else if (width > 0)
            {
                var resizeFilter = new ResizeFilter
                {
                    Mode = ResizeMode.UseWidth,
                    Width = Unit.Pixel(width)
                };
                imageLayer.Filters.Add(resizeFilter);
            }
            else if (height > 0)
            {
                var resizeFilter = new ResizeFilter
                {
                    Mode = ResizeMode.UseHeight,
                    Height = Unit.Pixel(height)
                };
                imageLayer.Filters.Add(resizeFilter);
            }

            // add the layer
            dynamicImage.Layers.Add(imageLayer);

            // generate url
            return ImageUrlGenerator.GetImageUrl(dynamicImage);
        }
    }
}