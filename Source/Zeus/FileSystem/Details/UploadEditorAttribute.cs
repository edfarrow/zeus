using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ormongo;
using Zeus.Design.Editors;

namespace Zeus.FileSystem.Details
{
	public class UploadEditorAttribute : AbstractEditorAttribute
	{
		public UploadEditorAttribute()
		{
			Title = "Upload";
		}

		[ValidationProperty("Name")]
		private class CompositeEditor : Control
		{
			public FileUpload Upload = new FileUpload();
			public TextBox ChangeName = new TextBox();

			public string Name
			{
				get { return (Upload != null) ? Upload.FileName : ChangeName.Text; }
			}

			protected override void OnInit(EventArgs e)
			{
				Upload.ID = "u";
				Controls.Add(Upload);
				ChangeName.ID = "n";
				Controls.Add(ChangeName);

				base.OnInit(e);
			}
		}

		public override bool UpdateItem(ContentItem item, Control editor)
		{
			CompositeEditor ce = editor as CompositeEditor;
			File f = item as File;
			if (ce.Upload.PostedFile != null && ce.Upload.PostedFile.ContentLength > 0)
			{
				f.Name = System.IO.Path.GetFileName(ce.Upload.PostedFile.FileName);
				f.Data = Attachment.Create(ce.Upload.PostedFile.InputStream, f.Name, ce.Upload.PostedFile.ContentType);
				f.Size = ce.Upload.PostedFile.ContentLength;
				return true;
			}
			else if (ce.ChangeName.Text.Length > 0)
			{
				f.Name = ce.ChangeName.Text;
			}
			return false;
		}

		protected override void UpdateEditorInternal(ContentItem item, Control editor)
		{
			CompositeEditor ce = editor as CompositeEditor;
			File f = item as File;
			if (f.Size != null)
			{
				ce.Upload.Visible = false;
				ce.ChangeName.Text = f.Name;
			}
			else
			{
				ce.ChangeName.Visible = false;
			}
		}

		protected override Control AddEditor(Control container)
		{
			CompositeEditor editor = new CompositeEditor();
			editor.ID = "compositeEditor";
			container.Controls.Add(editor);
			return editor;
		}
	}
}
