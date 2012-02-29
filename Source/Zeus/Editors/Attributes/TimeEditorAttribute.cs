﻿using System.Web.UI;
using Ext.Net;
using Zeus.ContentTypes;
using Zeus.EditableTypes;

namespace Zeus.Editors.Attributes
{
	/// <summary>
	/// Decorates the content item with a time editor.
	/// </summary>
	public class TimeEditorAttribute : AbstractEditorAttribute
	{
		public TimeEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{
			
		}

		protected override Control AddEditor(Control container)
		{
			TimeField range = new TimeField { ID = Name };
			container.Controls.Add(range);
			return range;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			TimeField range = (TimeField)editor;
			range.Text = (string) item[Name];
		}

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			TimeField range = (TimeField) editor;
			if ((string) item[Name] != range.Text)
			{
				item[Name] = range.Text;
				return true;
			}
			return false;
		}
	}
}