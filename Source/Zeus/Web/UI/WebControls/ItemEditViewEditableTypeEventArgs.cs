using System;
using Zeus.ContentTypes;
using Zeus.EditableTypes;

namespace Zeus.Web.UI.WebControls
{
	public class ItemEditViewEditableTypeEventArgs : EventArgs
	{
		public EditableType EditableType { get; set; }

		public ItemEditViewEditableTypeEventArgs(EditableType editableType)
		{
			EditableType = editableType;
		}
	}
}