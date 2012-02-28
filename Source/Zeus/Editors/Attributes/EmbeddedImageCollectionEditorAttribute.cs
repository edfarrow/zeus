using Zeus.Editors.Controls;
using Zeus.FileSystem;
using Zeus.FileSystem.Images;

namespace Zeus.Editors.Attributes
{
	public class EmbeddedImageCollectionEditorAttribute : EmbeddedFileCollectionEditorAttribute
	{
		public EmbeddedImageCollectionEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		protected override void HandleUpdatedFile(EmbeddedFile file)
		{
			Context.Current.Resolve<ImageCachingService>().DeleteCachedImages(file.Data);
		}

		protected override EmbeddedCollectionEditorBase CreateEditor()
		{
			return new MultiImageUploadEditor();
		}
	}
}