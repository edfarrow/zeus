using System;
using System.Web.Routing;
using CKEditor.NET;
using Ext.Net;
using Zeus.Editors.Resources;
using Zeus.Web.Routing;

namespace Zeus.Editors.Controls
{
	public class HtmlTextBox : CKEditorControl
	{
		static HtmlTextBox()
		{
			var assembly = typeof(EditorsResources).Assembly;
			var routeHandler = new EmbeddedContentRouteHandler(assembly, assembly.GetName().Name + ".CKEditor");
			RouteTable.Routes.Add(new Route(GetBasePath() + "/{*resource}", new RouteValueDictionary(), routeHandler));
		}

		private static string GetBasePath()
		{
			return Zeus.Context.AdminManager.AdminPath + "/assets/ckeditor";
		}

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
			BasePath = "/" + GetBasePath();
			BaseHref = Page.Request.Url.GetLeftPart(UriPartial.Authority);
			base.OnPreRender(e);
		}
	}
}