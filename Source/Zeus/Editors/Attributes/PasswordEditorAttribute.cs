using System.Web.UI.WebControls;
using Zeus.ContentTypes;
using Zeus.EditableTypes;
using Zeus.Web.Security;

namespace Zeus.Editors.Attributes
{
	public class PasswordEditorAttribute : TextBoxEditorAttribute
	{
		public PasswordEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		protected override void UpdateEditorInternal(IEditableObject item, System.Web.UI.Control editor)
		{
			
		}

		public override bool UpdateItem(IEditableObject item, System.Web.UI.Control editor)
		{
			TextBox tb = editor as TextBox;
			string value = (tb.Text == DefaultValue) ? null : tb.Text;
			if (string.IsNullOrEmpty(value))
				return false;
			value = Context.Current.Resolve<ICredentialService>().EncryptPassword(value);
			if (!AreEqual(value, item[Name]))
			{
				item[Name] = value;
				return true;
			}
			return false;
		}
	}
}