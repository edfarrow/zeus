using System;
using System.Web.UI;
using Zeus.ContentTypes;
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
			var itemEditor = new ItemEditView { ID = Name };
			container.Controls.Add(itemEditor);
			return itemEditor;
		}

		protected override Control AddEditor(Control container)
		{
			throw new NotImplementedException();
		}

		public override bool UpdateItem(IEditableObject item, Control editor)
		{
			var itemEditor = (ItemEditView)editor;
			item[Name] = itemEditor.CurrentItem;
			return true;
		}

		protected override void UpdateEditorInternal(IEditableObject item, Control editor)
		{
			var itemEditor = (ItemEditView) editor;
			var currentItem = (IEditableObject) item[Name] ??
				(IEditableObject) Activator.CreateInstance(UnderlyingProperty.PropertyType);
			itemEditor.CurrentItem = currentItem;
		}
	}
}