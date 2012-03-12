using Zeus.Editors.Attributes;
using Zeus.FileSystem;
using Zeus.Templates.ContentTypes;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
	[ContentType("MyPage", IgnoreSEOAssets=true)]
	[Panel("NewContainer", "All My Types Go in Here!", 100)]
	public class MyPage : BasePage
	{
		[AutoEditor("Product", 99)]
		public virtual ContentItem Product { get; set; }

		[EmbeddedImageEditor("Image", 100)]
		public virtual EmbeddedFile Image { get; set; }

		[EmbeddedImageEditor("Another Image", 110)]
		public virtual EmbeddedFile AnotherImage { get; set; }

        /*
        [ContentProperty("Other Images", 200, EditorContainerName = "Content")]
        [ChildrenEditor("Other Images", 200, TypeFilter = typeof(Zeus.FileSystem.Images.Image), ContainerName = "Content")]
        public IEnumerable<Zeus.FileSystem.Images.Image> Images
        {
            get { return GetChildren<Zeus.FileSystem.Images.Image>(); }
        }
         */


		/*[ChildrenEditor("Test Child Editors", 15, TypeFilter = typeof(MyLittleType), ContainerName = "NewContainer")]
		public virtual IEnumerable<MyLittleType> ListFilters
		{
			get { return GetChildren<MyLittleType>(); }
		}*/

        public override bool AllowParamsOnIndex
        {
			get { return true; }
        }
	}
}
