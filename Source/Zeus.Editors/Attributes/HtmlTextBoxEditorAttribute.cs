using System;
using System.Web.Routing;
using System.Web.UI;
using Zeus.Design.Editors;
using Zeus.Editors.Controls;
using Zeus.Web.Routing;

namespace Zeus.Editors.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class HtmlTextBoxEditorAttribute : TextEditorAttributeBase
	{
		static HtmlTextBoxEditorAttribute()
		{
			var assembly = typeof(HtmlTextBoxEditorAttribute).Assembly;
			var routeHandler = new EmbeddedContentRouteHandler(assembly, assembly.GetName().Name + ".Resources.CKEditor");
			RouteTable.Routes.Add(new Route(GetBasePath() + "/{*resource}", new RouteValueDictionary(), routeHandler));
		}

		private static string GetBasePath()
		{
			return Context.AdminManager.AdminPath + "/assets/ckeditor";
		}

		public HtmlTextBoxEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
		}

		public HtmlTextBoxEditorAttribute(string title, int sortOrder, int maxLength)
			: base(title, sortOrder, maxLength)
		{
		}

		public HtmlTextBoxEditorAttribute()
		{
		}

		public string RootHtmlElementID { get; set; }
		public string CustomCssUrl { get; set; }
		public string CustomStyleList { get; set; }

		/// <summary>Creates a text box editor.</summary>
		/// <param name="container">The container control the textbox will be placed in.</param>
		/// <returns>A textbox control.</returns>
		protected override Control AddEditor(Control container)
		{
			var tb = new HtmlTextBox
			{
				ID = Name,
				BasePath = "/" + GetBasePath(),
				FilebrowserBrowseUrl = "fileBrowserCallBack",
				Width = 800,
				Height = 400
			};
			if (!string.IsNullOrEmpty(CustomCssUrl))
				tb.ContentsCss = CustomCssUrl;
			if (!string.IsNullOrEmpty(CustomStyleList))
				tb.StylesSet = CustomStyleList;
			if (!string.IsNullOrEmpty(RootHtmlElementID))
				tb.BodyId = RootHtmlElementID;

			if (Required)
				tb.CssClass += " required";
			if (ReadOnly)
				tb.ReadOnly = true;

			container.Controls.Add(tb);
			return tb;
		}
	}
}