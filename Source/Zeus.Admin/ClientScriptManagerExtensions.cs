using System.Web.UI;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;

namespace Zeus.Admin
{
	public static class ClientScriptManagerExtensions
	{
		public static void RegisterJQuery(this ClientScriptManager clientScriptManager)
		{
			clientScriptManager.RegisterJavascriptResource(typeof(ClientScriptManagerExtensions), "Zeus.Admin.Assets.JS.jquery.js", ResourceInsertPosition.HeaderTop);
			clientScriptManager.RegisterClientScriptBlock(typeof(ClientScriptManagerExtensions), "JQueryNoConflict", "jQuery.noConflict();", true);
		}
	}
}