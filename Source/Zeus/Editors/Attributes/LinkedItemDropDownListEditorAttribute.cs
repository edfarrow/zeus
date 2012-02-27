﻿using System;
using System.Linq;
using System.Web.UI.WebControls;
using MongoDB.Bson;
using Zeus.BaseLibrary.ExtensionMethods.Linq;
using System.Web.UI;
using Zeus.ContentTypes;

namespace Zeus.Editors.Attributes
{
    public class LinkedItemDropDownListEditor : DropDownListEditorAttribute
	{
		#region Constructors

        public LinkedItemDropDownListEditor()
		{
			ExcludeSelf = true;
		}

		public LinkedItemDropDownListEditor(string title, int sortOrder)
			: base(title, sortOrder)
		{
			ExcludeSelf = true;
		}

		#endregion

        public bool ExcludeSelf { get; set; }
        public bool UseNonHiearchicalTitle { get; set; }
        public Type TypeFilter { get; set; }
        public bool IsRequired { get; set; }

		protected override object GetValue(ListControl ddl)
		{
			if (!string.IsNullOrEmpty(ddl.SelectedValue))
				return ContentItem.Find(ObjectId.Parse(ddl.SelectedValue));
			return null;
		}

		protected override object GetValue(IEditableObject item)
		{
			ContentItem linkedItem = (ContentItem) item[Name];
			if (linkedItem != null)
				return linkedItem.ID.ToString();
			return string.Empty;
		}

		public override bool UpdateItem(IEditableObject item, Control editor)
        {
            ListControl ddl = (ListControl)editor;
            object one = GetValue(ddl);
            object two = GetValue(item);
            
            if (one == null && two.ToString() == string.Empty)
            {//do nothing - this means the same as them being equal
            }
            else if (one == null && !IsRequired)
            {
               //the below case converting one to a ContentItem will error (and thus prevents the item from being saved), so set to null
                item[Name] = null;
                return true;
            }
            else if (one == null && IsRequired)
            {
                //throw a better error than the one which would be thrown
                throw (new Exception("This parameter (in the dropdownlist) is compulsory"));
            }
            else if (((ContentItem)one).ID.ToString() == two.ToString())
            {//do nothing - this means the same as them being equal
            }
            else if (GetValue(ddl) != GetValue(item))
            {
                item[Name] = GetValue(ddl);
                return true;
            }

            return false;
        }

		protected override ListItem[] GetListItems(IEditableObject item)
		{
			var contentItem = (ContentItem)item;
			IQueryable<ContentItem> items = ContentItem.All();
			if (TypeFilter != null)
				items = items.OfType(TypeFilter);
			if (ExcludeSelf)
				items = items.Where(i => i.ID != contentItem.ID);
			return items.ToList().OrderBy(i => i.HierarchicalTitle)
                .Select(i => new ListItem { Value = i.ID.ToString(), Text = UseNonHiearchicalTitle ? i.Title : i.HierarchicalTitle })
				.ToArray();
		}
	}
}