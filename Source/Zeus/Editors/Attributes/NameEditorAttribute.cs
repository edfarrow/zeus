﻿using System;
using System.Web.UI;
using Zeus.ContentTypes;
using Zeus.Editors.Controls;

[assembly: WebResource("NameEditorAttribute.js", "text/javascript")]

namespace Zeus.Editors.Attributes
{
	public class NameEditorAttribute : AbstractEditorAttribute
	{
		public NameEditorAttribute(string title, int sortOrder)
			: base(title, "Name", sortOrder)
		{
			Required = true;
		}

		protected override Control AddEditor(Control container)
		{
			NameEditor nameEditor = new NameEditor { ID = "txtNameEditor" };
			if (Required)
				nameEditor.CssClass += " required";
			container.Controls.Add(nameEditor);
			return nameEditor;
		}

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
            object editorValue = ((NameEditor)editor).Text;
            object itemValue = item[Name];
            if (!AreEqual(editorValue, itemValue))
            {
                item[Name] = ((NameEditor)editor).Text;
                return true;
            }
            return false;			
		}

		protected override void UpdateEditorInternal(ContentItem item, Control editor)
		{
			NameEditor ne = (NameEditor) editor;
			ne.Text = item.Name;
			ne.Prefix = "/";
			ne.Suffix = item.Extension;
			try
			{
				if (Context.UrlParser.StartPage == item || item.Parent == null)
				{
					ne.Prefix = string.Empty;
					ne.Suffix = string.Empty;
				}
				else if (Context.UrlParser.StartPage != item.Parent)
				{
					string parentUrl = item.Parent.Url;
					if (!parentUrl.Contains("?"))
					{
						string prefix = parentUrl;
						if (!string.IsNullOrEmpty(item.Extension))
						{
							int aspxIndex = parentUrl.IndexOf(item.Extension, StringComparison.InvariantCultureIgnoreCase);
							prefix = parentUrl.Substring(0, aspxIndex);
						}
						prefix += "/";
						if (prefix.Length > 60)
							prefix = prefix.Substring(0, 50) + ".../";
						ne.Prefix = prefix;
					}
				}
			}
			catch (Exception)
			{
			}
		}
	}
}