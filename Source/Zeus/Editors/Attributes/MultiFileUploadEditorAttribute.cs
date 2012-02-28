using System;
using System.IO;
using System.Web.UI;
using Ormongo;
using Zeus.BaseLibrary.ExtensionMethods.IO;
using Zeus.BaseLibrary.Web;
using Zeus.EditableTypes;
using Zeus.Editors.Controls;
using Zeus.FileSystem;
using Zeus.Web.Handlers;
using File = Zeus.FileSystem.File;

namespace Zeus.Editors.Attributes
{
	public class MultiFileUploadEditorAttribute : EmbeddedCollectionEditorAttributeBase
	{
		public MultiFileUploadEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		protected override void CreateOrUpdateItem(IEditableObject item, object existingValue, Control editor, out object newValue)
		{
			var contentItem = (ContentItem) item;
			FancyFileUpload fileEditor = (FancyFileUpload)editor;
			if (fileEditor.HasNewOrChangedFile)
			{
				// Add new file.
				File newFile = existingValue as File;
				if (newFile == null)
				{
					newFile = CreateNewItem();
					newFile.Name = Name + Guid.NewGuid();
					newFile.Parent = contentItem;
				}

				// Populate FileData object.
				newFile.FileName = fileEditor.FileName;
				string uploadFolder = BaseFileUploadHandler.GetUploadFolder(fileEditor.Identifier);
				string uploadedFile = Path.Combine(uploadFolder, fileEditor.FileName);
				using (FileStream fs = new FileStream(uploadedFile, FileMode.Open))
				{
					byte[] data = fs.ReadAllBytes();
					fs.Position = 0;
					newFile.Data = new EmbeddedFile
					{
						Data = Attachment.Create(new Attachment { Content = fs, FileName = fileEditor.FileName, ContentType = MimeUtility.GetMimeType(data) })
					};
					newFile.Size = fs.Length;
				}

				// Delete temp folder.
				System.IO.File.Delete(uploadedFile);
				Directory.Delete(uploadFolder);

				newValue = newFile;

				if (existingValue != null)
					HandleUpdatedFile(newFile);
			}
			else
			{
				newValue = null;
			}
		}

		protected virtual void HandleUpdatedFile(File file)
		{
			
		}

		protected virtual File CreateNewItem()
		{
			return new File();
		}

		protected override EmbeddedCollectionEditorBase CreateEditor()
		{
			return new MultiFileUploadEditor();
		}
	}
}