using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using Zeus.BaseLibrary.ExtensionMethods.Linq;
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
			get { return Zeus.Context.Current.ContentTypes.GetContentType(TypeFilterInternal); }
		}

		protected override string ItemTitle
		{
			get { return ContentType.Title; }
		}

		#endregion

		protected override Control CreateDetailEditor(int id, object detail)
		{
			DropDownList ddl = new DropDownList { CssClass = "linkedItem", ID = ID + "_ddl_" + id };
			IEnumerable<ContentItem> contentItems = ContentItem.All().OfType(TypeFilterInternal);
			ddl.Items.AddRange(contentItems.Select(ci => new ListItem(ci.Title, ci.ID.ToString())).ToArray());
			if (detail != null)
				ddl.SelectedValue = detail.ToString();
			return ddl;
		}
	}
}
