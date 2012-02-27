using System;
using CKEditor.NET;
using Ext.Net;

namespace Zeus.Editors.Controls
{
	public class HtmlTextBox : CKEditorControl
	{
		protected override void OnPreRender(EventArgs e)
		{
			ExtNet.ResourceManager.RegisterClientScriptBlock("HtmlTextBoxFileBrowserCallback", 
				@"function fileBrowserCallBack(fieldName, url, destinationType, win)
				{
					var srcField = win.document.forms[0].elements[fieldName];
					var insertFileUrl = function(data) {
						srcField.value = data.url;
					};

					top.fileManager.show(Ext.get(fieldName), insertFileUrl, destinationType);
				}");
			ExtNet.ResourceManager.RegisterClientStyleBlock("HtmlTextBox", ".cke_1 { float: left; }");
			BaseHref = Page.Request.Url.GetLeftPart(UriPartial.Authority);
			base.OnPreRender(e);
		}
	}
}