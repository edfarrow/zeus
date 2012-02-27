using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.ContentTypes;
using Zeus.Editors.Controls;

namespace Zeus.Editors.Attributes
{
	public class StringCollectionEditorAttribute : BaseDetailCollectionEditorAttribute
	{
		public StringCollectionEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		protected override BaseDetailCollectionEditor CreateEditor()
		{
			return new StringCollectionEditor { ID = Name };
		}

		protected override void CreateOrUpdateDetailCollectionItem(IEditableObject item, object existingDetail, Control editor, out object newDetail)
		{
			newDetail = ((TextBox) editor).Text;
		}
	}
}