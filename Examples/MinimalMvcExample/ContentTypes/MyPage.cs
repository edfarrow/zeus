using System.Collections.Generic;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Web.UI;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes
{
	[ContentType("MyPage", IgnoreSEOAssets=true)]
	[AllowedChildren(typeof(Zeus.FileSystem.Images.Image))]
	[Panel("NewContainer", "All My Types Go in Here!", 100)]
	[FieldSet("ImageContainer", "Main Image", 200, ContainerName = "Content")]
	[FieldSet("AnotherImageContainer", "Secondary Image", 300, ContainerName = "Content")]
	public class MyPage : BasePage
	{
		[LinkedItemDropDownListEditor("Product", 99)]
		public virtual ContentItem Product { get; set; }

		[ChildEditor("Image", 100, ContainerName="ImageContainer")]
		public virtual Zeus.FileSystem.Images.Image Image
		{
			get { return GetChild("Image") as Zeus.FileSystem.Images.Image; }
			set
			{
				if (value != null)
				{
					value.Name = "Image";
					value.Parent = this;
				}
			}
		}

		[ChildEditor("Another Image", 110, ContainerName = "AnotherImageContainer")]
		public virtual Zeus.FileSystem.Images.Image AnotherImage
		{
			get { return GetChild("AnotherImage") as Zeus.FileSystem.Images.Image; }
			set
			{
				if (value != null)
				{
					value.Name = "AnotherImage";
					value.Parent = this;
				}
			}
		}

        /*
        [ContentProperty("Other Images", 200, EditorContainerName = "Content")]
        [ChildrenEditor("Other Images", 200, TypeFilter = typeof(Zeus.FileSystem.Images.Image), ContainerName = "Content")]
        public IEnumerable<Zeus.FileSystem.Images.Image> Images
        {
            get { return GetChildren<Zeus.FileSystem.Images.Image>(); }
        }
         */


		[ChildrenEditor("Test Child Editors", 15, TypeFilter = typeof(MyLittleType), ContainerName = "NewContainer")]
		public virtual IEnumerable<MyLittleType> ListFilters
		{
			get { return GetChildren<MyLittleType>(); }
		}

        public override bool AllowParamsOnIndex
        {
            get
            {
                return true;
            }
        }

	}
}
