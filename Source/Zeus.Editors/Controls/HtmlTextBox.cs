using System;
using CKEditor.NET;
using Ext.Net;

namespace Zeus.Editors.Controls
{
	public class HtmlTextBox : CKEditorControl
	{
		protected override void OnPreRender(EventArgs e)
		{
			ExtNet.ResourceManager.RegisterClientStyleBlock("HtmlTextBox", ".cke_1 { float: left; }");
			BaseHref = Page.Request.Url.GetLeftPart(UriPartial.Authority);
			base.OnPreRender(e);
		}
	}
}