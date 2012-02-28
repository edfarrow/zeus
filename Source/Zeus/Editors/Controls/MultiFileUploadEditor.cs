using System.Web.UI;
using Zeus.FileSystem;

namespace Zeus.Editors.Controls
{
	public class MultiFileUploadEditor : EmbeddedCollectionEditorBase
	{
		#region Properties

		protected override string ItemTitle
		{
			get { return "File"; }
		}

		#endregion

		protected override Control CreateValueEditor(int id, object value)
		{
			FancyFileUpload fileUpload = CreateEditor();
			fileUpload.ID = ID + "_upl_" + id;

			if (value != null)
				fileUpload.CurrentFileName = ((EmbeddedFile) value).Data.FileName;

			return fileUpload;
		}

		protected virtual FancyFileUpload CreateEditor()
		{
			return new FancyFileUpload();
		}
	}
}