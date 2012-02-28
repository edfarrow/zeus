using System;
using Zeus.Editors.Controls;
using Zeus.FileSystem;
using Zeus.FileSystem.Images;

namespace Zeus.Editors.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class EmbeddedImageEditorAttribute : EmbeddedFileEditorAttribute
	{
		public int? MinimumWidth { get; set; }
		public int? MinimumHeight { get; set; }

		/// <summary>Initializes a new instance of the ImageUploadEditorAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		public EmbeddedImageEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{

		}

		protected override FancyFileUpload CreateEditor()
		{
			return new FancyImageUpload { MinimumWidth = MinimumWidth, MinimumHeight = MinimumHeight };
		}

		protected override void HandleUpdatedFile(EmbeddedFile file)
		{
			Context.Current.Resolve<ImageCachingService>().DeleteCachedImages(file.Data);
		}
	}
}