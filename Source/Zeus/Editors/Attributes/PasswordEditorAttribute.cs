using System.Web.UI.WebControls;
using Zeus.Web.Security;

namespace Zeus.Design.Editors
{
	public class PasswordEditorAttribute : TextBoxEditorAttribute
	{
		public PasswordEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		protected override void UpdateEditorInternal(ContentItem item, System.Web.UI.Control editor)
		{
			
		}

		public override bool UpdateItem(ContentItem item, System.Web.UI.Control editor)
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