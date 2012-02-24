using System;
using System.Web.UI;
using Zeus.FileSystem.Images;

namespace Zeus.Design.Editors
{
	[AttributeUsage(AttributeTargets.Property)]
	public class CroppedImageAttachmentEditorAttribute : ImageAttachmentEditorAttribute
	{
		/// <summary>Initializes a new instance of the ImageUploadEditorAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		public CroppedImageAttachmentEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{

		}

        protected override void UpdateEditorInternal(ContentItem item, Control editor)
        {
            base.UpdateEditorInternal(item, editor);

            if (((CroppedImage)item).Data != null)
            {
                //check to see if the image is large enough...
                CroppedImage image = (CroppedImage)item;

                System.Drawing.Image imageForSize = System.Drawing.Image.FromStream(image.Data.Content);
                int actualWidth = imageForSize.Width;
                int actualHeight = imageForSize.Height;
                imageForSize.Dispose();

                if (actualWidth > image.FixedWidthValue && actualHeight > image.FixedHeightValue)
                {
                    string selected = System.Web.HttpContext.Current.Request.QueryString["selected"];
                    editor.Controls.AddAt(editor.Controls.Count, new LiteralControl("<div><p>Preview of how the image will look on the page</p><br/><p><a href=\"/admin/ImageCrop.aspx?id=" + image.ID + "&selected=" + selected + "\">Edit Crop</a></p><br/>"));
                    editor.Controls.AddAt(editor.Controls.Count, new LiteralControl("<img src=\"" + ((CroppedImage)image).GetUrl(image.FixedWidthValue, image.FixedHeightValue, true, SoundInTheory.DynamicImage.DynamicImageFormat.Jpeg, false) + "?rand=" + new System.Random().Next(1000) + "\" /></div><br/><br/>"));
                }
                else
                {
                    editor.Controls.AddAt(editor.Controls.Count, new LiteralControl("<div><p>Image is not large enough to be cropped - it is advised that you upload a larger image</p><br/>"));                    
                }
            }
        }
	}
}