using System;
using System.Collections.Generic;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;
using Zeus.ContentTypes;
using Zeus.Integrity;
using Zeus.Security;
using ListItem=System.Web.UI.WebControls.ListItem;

namespace Zeus.Admin.Plugins.NewItem
{
	[ActionPluginGroup("NewEditDelete", 10)]
	[AvailableOperation(Operations.Create, "Create", 20)]
	public partial class Default : PreviewFrameAdminPage
	{
		private ContentType ParentItemDefinition = null;

		public ContentItem ActualItem
		{
			get { return base.SelectedItem.Parent; }
		}

		protected void Page_Init(object sender, EventArgs e)
		{
			hlCancel.NavigateUrl = CancelUrl();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			ParentItemDefinition = Engine.ContentTypes.GetContentType(ActualItem.GetType());
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			ContentItem parentItem = SelectedItem;
			IContentTypeManager manager = Zeus.Context.Current.Resolve<IContentTypeManager>();
			ContentType contentType = manager.GetContentType(parentItem.GetType());
			lsvChildTypes.DataSource = manager.GetAllowedChildren(contentType, User);
			lsvChildTypes.DataBind();
		}

		protected string GetEditUrl(ContentType contentType)
		{
			return Engine.AdminManager.GetEditNewPageUrl(SelectedItem, contentType);
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.new.css");
			LoadAllowedTypes();
			base.OnPreRender(e);
		}

		private void LoadAllowedTypes()
		{
			int allowedChildrenCount = ParentItemDefinition.AllowedChildren.Count;
			IList<ContentType> allowedChildren = Engine.ContentTypes.GetAllowedChildren(ParentItemDefinition, User);

			if (allowedChildrenCount == 0)
			{
				Title = string.Format("No item is allowed below an item of type '{0}'", ParentItemDefinition.Title);
			}
			else if (allowedChildrenCount == 1 && allowedChildren.Count == 1)
			{
				Response.Redirect(GetEditUrl(allowedChildren[0]));
			}
			else
			{
				Title = string.Format("Select type of item below '{0}'", ActualItem.Title);

				lsvChildTypes.DataSource = allowedChildren;
				lsvChildTypes.DataBind();
			}
		}
	}
}
