﻿using System;
using System.IO;
using System.Web.UI;
using Isis.ExtensionMethods.IO;
using Isis.Web;
using Zeus.ContentTypes;
using Zeus.FileSystem;
using Zeus.Web.Handlers;
using Zeus.Web.UI.WebControls;

namespace Zeus.Design.Editors
{
	[AttributeUsage(AttributeTargets.Property)]
	public class FileDataUploadEditorAttribute : AbstractEditorAttribute
	{
		/// <summary>Initializes a new instance of the FileUploadEditorAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		public FileDataUploadEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			UploadMethod = UploadMethod.Flash;
		}

		public FileDataUploadEditorAttribute()
		{
			UploadMethod = UploadMethod.Flash;
		}

		protected virtual FileData CreateNewItem()
		{
			return new FileData();
		}

		public UploadMethod UploadMethod { get; set; }

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			FileDataEditor fileEditor = (FileDataEditor) editor;
			FileData existingFile = item[Name] as FileData;

			bool result = false;
			if (fileEditor.HasNewOrChangedFile)
			{
				// Add new file.
				FileData newFile = existingFile ?? CreateNewItem();

				// Populate FileData object.
				newFile.FileName = fileEditor.FileName;
				string uploadFolder = BaseFileUploadHandler.GetUploadFolder(fileEditor.Identifier);
				string uploadedFile = Path.Combine(uploadFolder, fileEditor.Page.Server.UrlDecode(fileEditor.FileName));
				using (FileStream fs = new FileStream(uploadedFile, FileMode.Open))
				{
					newFile.Data = fs.ReadAllBytes();
					newFile.ContentType = MimeUtility.GetMimeType(newFile.Data);
					newFile.Size = fs.Length;
				}

				// Delete temp folder.
				System.IO.File.Delete(uploadedFile);
				Directory.Delete(uploadFolder);

				item[Name] = newFile;

				result = true;
			}

			if (OnItemUpdated(item, editor))
				result = true;

			return result;
		}

		protected virtual bool OnItemUpdated(IEditableObject item, Control editor)
		{
			/*FileEditor fileUpload = (FileEditor) editor;
			if (fileUpload.ShouldClear)
			{
				item[Name] = null;
				return true;
			}
			*/
			return false;
		}

		/// <summary>Creates a text box editor.</summary>
		/// <param name="container">The container control the tetx box will be placed in.</param>
		/// <returns>A text box control.</returns>
		protected override Control AddEditor(Control container)
		{
			FileDataEditor fileUpload = CreateEditor();
			fileUpload.ID = Name;
			fileUpload.FileUploadImplementation = CreateUpload(fileUpload);
			container.Controls.Add(fileUpload);

			return fileUpload;
		}

		protected override void DisableEditor(Control editor)
		{
			((FileDataEditor) editor).Enabled = false;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			FileData file = item[Name] as FileData;
			if (file != null)
			{
				FileDataEditor fileUpload = (FileDataEditor) editor;
				fileUpload.CurrentFileName = file.FileName;
			}
		}

		protected virtual FileDataEditor CreateEditor()
		{
			return new FileDataEditor();
		}

		protected FileUploadImplementation CreateUpload(FileDataEditor fileUpload)
		{
			switch (UploadMethod)
			{
				case UploadMethod.Flash:
					return new FlashFileUploadImplementation(fileUpload);
				default :
					throw new NotSupportedException();
			}
		}
	}

	public enum UploadMethod
	{
		Flash
	}
}