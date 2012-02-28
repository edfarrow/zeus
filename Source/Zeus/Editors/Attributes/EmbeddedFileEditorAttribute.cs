using System;
using System.Web.UI;
using Ormongo;
using Zeus.EditableTypes;
using Zeus.Editors.Controls;
using Zeus.FileSystem;

namespace Zeus.Editors.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class EmbeddedFileEditorAttribute : AbstractEditorAttribute
	{
		/// <summary>Initializes a new instance of the EditableTextBoxAttribute class.</summary>
		/// <param name="title">The label displayed to editors</param>
		/// <param name="sortOrder">The order of this editor</param>
		public EmbeddedFileEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{

		}

		#region Properties

		public string TypeFilterDescription { get; set; }
		public string[] TypeFilter { get; set; }
		public int MaximumFileSize { get; set; }

		#endregion

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			var fileUpload = (FancyFileUpload)editor;
			var file = (EmbeddedFile)item[Name];

			bool result = false;
			if (fileUpload.HasDeletedFile)
			{
				Attachment.Delete(file.Data.ID);
				item[Name] = null;
				result = true;
			}
			else if (fileUpload.HasNewOrChangedFile)
			{
				var embeddedFile = file ?? CreateEmbeddedFile();
				FileUploadUtility.HandleUpload(fileUpload, embeddedFile);
				item[Name] = embeddedFile;

				result = true;
			}

			if (file != null)
				HandleUpdatedFile(file);

			return result;
		}

		protected virtual EmbeddedFile CreateEmbeddedFile()
		{
			return new EmbeddedFile();
		}

		protected virtual void HandleUpdatedFile(EmbeddedFile file)
		{

		}

		/// <summary>Creates a text box editor.</summary>
		/// <param name="container">The container control the tetx box will be placed in.</param>
		/// <returns>A text box control.</returns>
		protected override Control AddEditor(Control container)
		{
			FancyFileUpload fileUpload = CreateEditor();
			fileUpload.ID = Name;
			if (!string.IsNullOrEmpty(TypeFilterDescription))
				fileUpload.TypeFilterDescription = TypeFilterDescription;
			if (TypeFilter != null)
				fileUpload.TypeFilter = TypeFilter;
			if (MaximumFileSize > 0)
				fileUpload.MaximumFileSize = MaximumFileSize;
			container.Controls.Add(fileUpload);

			return fileUpload;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			var fileUpload = (FancyFileUpload)editor;
			var file = (EmbeddedFile)item[Name];
			if (file != null)
				fileUpload.CurrentFileName = file.Data.FileName;
		}

		protected virtual FancyFileUpload CreateEditor()
		{
			return new FancyFileUpload();
		}
	}
}