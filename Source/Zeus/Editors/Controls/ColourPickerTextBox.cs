using System;
using System.Web.UI.WebControls;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;
using Zeus.Editors.Resources;

namespace Zeus.Editors.Controls
{
	public class ColourPickerTextBox : TextBox
	{
		public ColourPickerTextBox()
		{
			CssClass = "colourPicker";
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);

			Page.ClientScript.RegisterJavascriptResource(typeof(EditorsResources), "Zeus.Editors.Resources.ColorPicker.jquery.colorpicker.js");
			Page.ClientScript.RegisterCssResource(typeof(EditorsResources), "Zeus.Editors.Resources.ColorPicker.jquery.colorpicker.css");

			string script = @"$('#" + this.ClientID + @"').ColorPicker({
	onSubmit: function(hsb, hex, rgb) {
		$('#" + this.ClientID + @"').val(hex);
	},
	onBeforeShow: function() {
		$(this).ColorPickerSetColor(this.value);
	}
})
.bind('keyup', function() {
	$(this).ColorPickerSetColor(this.value);
});";
			Page.ClientScript.RegisterStartupScript(typeof(ColourPickerTextBox), ClientID, script, true);
		}
	}
}
