using System.Web.UI;
using System.Web.UI.WebControls;

namespace Zeus.Web.UI.WebControls
{
	public class StringCollectionEditor : BaseDetailCollectionEditor
	{
		#region Properties

		protected override string ItemTitle
		{
			get { return "String"; }
		}

		#endregion

		protected override Control CreateDetailEditor(int id, object detail)
		{
			TextBox txt = new TextBox { CssClass = "linkedItem", ID = ID + "_txt_" + id };
			if (detail != null)
				txt.Text = detail.ToString();
			return txt;
		}
	}
}