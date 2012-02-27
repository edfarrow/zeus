using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using MongoDB.Bson;
using Zeus.ContentTypes;
using Zeus.Editors.Controls;

namespace Zeus.Editors.Attributes
{
	public class LinkedItemsEditorAttribute : BaseDetailCollectionEditorAttribute
	{
		public LinkedItemsEditorAttribute(string title, int sortOrder, Type typeFilter)
			: base(title, sortOrder)
		{
			TypeFilter = typeFilter;
		}

		public Type TypeFilter { get; set; }

		protected override BaseDetailCollectionEditor CreateEditor()
		{
			return new LinkedItemsEditor { ID = Name, TypeFilter = TypeFilter.ToString() };
		}

		protected override void CreateOrUpdateDetailCollectionItem(IEditableObject item, object existingDetail, Control editor, out object newDetail)
		{
			DropDownList ddl = (DropDownList) editor;
			newDetail = ContentItem.Find(ObjectId.Parse(ddl.SelectedValue));
		}
	}
}