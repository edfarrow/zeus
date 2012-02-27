using System;
using System.Web.UI;
using Zeus.Design.Editors;
using Zeus.Editors.Controls;

namespace Zeus.Editors.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class HtmlTextBoxEditorAttribute : TextEditorAttributeBase
	{
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