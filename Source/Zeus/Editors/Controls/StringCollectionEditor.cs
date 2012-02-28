using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeus.Editors.Controls
{
	public class StringCollectionEditor : EmbeddedCollectionEditorBase
	{
		protected override string ItemTitle
		{
			get { return "String"; }
		}

		protected override Control CreateValueEditor(int id, object value)
		{
			var txt = new TextBox { CssClass = "linkedItem", ID = ID + "_txt_" + id };
			if (value != null)
				txt.Text = value.ToString();
			return txt;
		}
	}
}