using System;
using System.Web.UI;
using Ext.Net;
using Zeus.EditableTypes;
using Zeus.Web.UI.WebControls;

namespace Zeus.Editors.Attributes
{
	[AttributeUsage(AttributeTargets.Property)]
	public class EmbeddedItemEditorAttribute : AbstractEditorAttribute
	{
		#region Constructors

		public EmbeddedItemEditorAttribute(string title, int sortOrder)
			: base(title, sortOrder)
		{

		}

		#endregion

		public override Control AddTo(Control container)
		{
			var panel = AddPanel(container);
			var itemEditor = new ItemEditor { ID = Name };
			panel.Controls.Add(itemEditor);	
			return itemEditor;
		}

		protected override Control AddEditor(Control container)
		{
			throw new NotImplementedException();
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			var itemEditor = (ItemEditor)editor;
			var currentItem = (IEditableObject)item[Name] ??
				(IEditableObject)Activator.CreateInstance(UnderlyingProperty.PropertyType);
			itemEditor.CurrentItem = currentItem;
		}

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			var itemEditor = (ItemEditor) editor;
			itemEditor.UpdateItem();
			item[Name] = itemEditor.CurrentItem;
			return true;
		}
	}
}