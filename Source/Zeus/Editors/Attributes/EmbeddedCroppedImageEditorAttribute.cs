using System;
using System.Web.UI;
using SoundInTheory.DynamicImage;
using Zeus.ContentTypes;
using Zeus.EditableTypes;
using Zeus.FileSystem;
using Zeus.FileSystem.Images;

namespace Zeus.Editors.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class EmbeddedCroppedImageEditorAttribute : EmbeddedImageEditorAttribute
	{
		public int FixedWidthValue { get; set; }
		public int FixedHeightValue { get; set; }

		/// <summary>Initializes a new instance of the ImageUploadEditorAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		public EmbeddedCroppedImageEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{

		}

        protected override void UpdateEditorInternal(IEditableObject item, Control editor)
        {
            base.UpdateEditorInternal(item, editor);

			var contentItem = (ContentItem)item;
			var embeddedFile = (EmbeddedCroppedImage)item[Name];

			if (embeddedFile != null)
            {
                //check to see if the image is large enough...
				System.Drawing.Image imageForSize = System.Drawing.Image.FromStream(embeddedFile.Data.Content);
                int actualWidth = imageForSize.Width;
                int actualHeight = imageForSize.Height;
                imageForSize.Dispose();

                if (actualWidth > FixedWidthValue && actualHeight > FixedHeightValue)
                {
                    string selected = System.Web.HttpContext.Current.Request.QueryString["selected"];
					editor.Controls.AddAt(editor.Controls.Count, new LiteralControl("<div><p>Preview of how the image will look on the page</p><br/><p><a href=\"/admin/ImageCrop.aspx?id=" + contentItem.ID + "&name=" + Name + "&selected=" + selected + "&w=" + FixedWidthValue + "&h=" + FixedHeightValue + "\">Edit Crop</a></p><br/>"));
                    editor.Controls.AddAt(editor.Controls.Count, new LiteralControl("<img src=\"" + embeddedFile.GetUrl(FixedWidthValue, FixedHeightValue, DynamicImageFormat.Jpeg, false) + "?rand=" + new Random().Next(1000) + "\" /></div><br/><br/>"));
                }
                else
                {
                    editor.Controls.AddAt(editor.Controls.Count, new LiteralControl("<div><p>Image is not large enough to be cropped - it is advised that you upload a larger image</p><br/>"));                    
                }
            }
        }

		protected override EmbeddedFile CreateEmbeddedFile()
		{
			return new EmbeddedCroppedImage();
		}
	}
}