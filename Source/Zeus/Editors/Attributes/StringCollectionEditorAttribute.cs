using System.Web.UI;
using System.Web.UI.WebControls;
using Zeus.EditableTypes;
using Zeus.Editors.Controls;

namespace Zeus.Editors.Attributes
{
	public class StringCollectionEditorAttribute : EmbeddedCollectionEditorAttributeBase
	{
		public StringCollectionEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		protected override EmbeddedCollectionEditorBase CreateEditor()
		{
			return new StringCollectionEditor { ID = Name };
		}

		protected override void CreateOrUpdateItem(IEditableObject item, object existingValue, Control editor, out object newValue)
		{
			newValue = ((TextBox) editor).Text;
		}
	}
}