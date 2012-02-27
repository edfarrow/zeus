using System.Web.UI;
using Zeus.FileSystem;

namespace Zeus.Editors.Controls
{
	public class MultiFileUploadEditor : BaseDetailCollectionEditor
	{
		#region Properties

		protected override string ItemTitle
		{
			get { return "File"; }
		}

		#endregion

		protected override Control CreateDetailEditor(int id, object detail)
		{
			FancyFileUpload fileUpload = CreateEditor();
			fileUpload.ID = ID + "_upl_" + id;

			if (detail != null)
				fileUpload.CurrentFileName = ((File) detail).FileName;

			return fileUpload;
		}

		protected virtual FancyFileUpload CreateEditor()
		{
			return new FancyFileUpload();
		}
	}
}