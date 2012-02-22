using System;
using System.Collections;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Zeus.ContentTypes;
using System.Web.UI;
using System.Web.Compilation;
using System.Linq;

namespace Zeus.Web.UI.WebControls
{
	public sealed class LinkedItemsEditor : BaseDetailCollectionEditor
	{
		#region Properties

		public string TypeFilter
		{
			get { return (string) ViewState["TypeFilter"]; }
			set { ViewState["TypeFilter"] = value; }
		}

		private Type TypeFilterInternal
		{
			get { return BuildManager.GetType(TypeFilter, true); }
		}

		private ContentType ContentType
		{
			get { return Zeus.Context.Current.ContentTypes[TypeFilterInternal]; }
		}

		protected override string ItemTitle
		{
			get { return ContentType.Title; }
		}

		#endregion

		protected override Control CreateDetailEditor(int id, object detail)
		{
			System.Web.UI.WebControls.DropDownList ddl = new System.Web.UI.WebControls.DropDownList { CssClass = "linkedItem", ID = ID + "_ddl_" + id };
			IEnumerable<ContentItem> first = ((IEnumerable) Zeus.Context.Current.Finder.Query(TypeFilterInternal)).Cast<ContentItem>();
			IEnumerable<ContentItem> contentItems = first.ToArray().Cast<ContentItem>();
			ddl.Items.AddRange(contentItems.Select(ci => new ListItem(ci.Title, ci.ID.ToString())).ToArray());
			if (detail != null)
				ddl.SelectedValue = detail.ToString();
			return ddl;
		}
	}
}
