using System.Collections.Generic;
using Ormongo;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Web.UI;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
	[ContentType("MyPage", IgnoreSEOAssets=true)]
	[Panel("NewContainer", "All My Types Go in Here!", 100)]
	public class MyPage : BasePage
	{
		[LinkedItemDropDownListEditor("Product", 99)]
		public virtual ContentItem Product { get; set; }

		[ImageAttachmentEditor("Image", 100)]
		public virtual Attachment Image { get; set; }

		[ImageAttachmentEditor("Another Image", 110)]
		public virtual Attachment AnotherImage { get; set; }

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
