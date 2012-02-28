using System.Web.UI;
using Zeus.EditableTypes;
using Zeus.Editors.Controls;
using Zeus.FileSystem;

namespace Zeus.Editors.Attributes
{
	public class EmbeddedFileCollectionEditorAttribute : EmbeddedCollectionEditorAttributeBase
	{
		public EmbeddedFileCollectionEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		protected override void CreateOrUpdateItem(IEditableObject item, object existingValue, Control editor, out object newValue)
		{
			var fileEditor = (FancyFileUpload)editor;
			if (fileEditor.HasNewOrChangedFile)
			{
				var newFile = existingValue as EmbeddedFile ?? CreateNewItem();

				FileUploadUtility.HandleUpload(fileEditor, newFile);
				newValue = newFile;

				if (existingValue != null)
					HandleUpdatedFile(newFile);
			}
			else
			{
				newValue = null;
			}
		}

		protected virtual void HandleUpdatedFile(EmbeddedFile file)
		{
			
		}

		protected virtual EmbeddedFile CreateNewItem()
		{
			return new EmbeddedFile();
		}

		protected override EmbeddedCollectionEditorBase CreateEditor()
		{
			return new MultiFileUploadEditor();
		}
	}
}