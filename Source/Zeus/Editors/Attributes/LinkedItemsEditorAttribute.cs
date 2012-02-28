using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MongoDB.Bson;
using Zeus.EditableTypes;
using Zeus.Editors.Controls;

namespace Zeus.Editors.Attributes
{
	public class LinkedItemsEditorAttribute : EmbeddedCollectionEditorAttributeBase
	{
		public LinkedItemsEditorAttribute(string title, int sortOrder, Type typeFilter)
			: base(title, sortOrder)
		{
			TypeFilter = typeFilter;
		}

		public Type TypeFilter { get; set; }

		protected override EmbeddedCollectionEditorBase CreateEditor()
		{
			return new LinkedItemsEditor
			{
				ID = Name,
				TypeFilter = TypeFilter.ToString()
			};
		}

		protected override void CreateOrUpdateItem(IEditableObject item, object existingValue, Control editor, out object newValue)
		{
			var ddl = (DropDownList) editor;
			newValue = ContentItem.Find(ObjectId.Parse(ddl.SelectedValue));
		}
	}
}