﻿using System;
using Ext.Net;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;
using Zeus.ContentTypes;
using Zeus.Editors.Resources;
using Zeus.Security;
using Zeus.Web;
using Zeus.Web.UI.WebControls;

namespace Zeus.Admin.Plugins.EditItem
{
	[ActionPluginGroup("NewEditDelete", 10)]
	[AvailableOperation(Operations.Create, "Create", 20)]
	[AvailableOperation(Operations.Change, "Change", 30)]
	public partial class Default : PreviewFrameAdminPage
	{
		private string Discriminator
		{
			get { return Request.QueryString["discriminator"]; }
		}

		protected override void OnInit(EventArgs e)
		{
			if (Discriminator != null)
			{
				Title = "New " + TypeDefinition.Title;
			}
			else
			{
				zeusItemEditView.CurrentItem = SelectedItem;
				Title = "Edit \"" + SelectedItem.Title + "\"";
			}

			base.OnInit(e);
		}

		protected string GetSessionKeepAliveUrl()
		{
			return Engine.GetServerResourceUrl(typeof(KeepAlive).Assembly, "Zeus.Admin.Plugins.EditItem.KeepAlive.aspx");
		}

		protected void btnSave_Click(object sender, DirectEventArgs e)
		{
			Validate();
			if (!IsValid)
				return;

			try
			{
				SaveChanges();
			}
			catch (Exception ex)
			{
				Engine.Resolve<IErrorHandler>().Notify(ex);
				ExtNet.MessageBox.Show(new MessageBoxConfig
				{
					Icon = MessageBox.Icon.ERROR,
					Buttons = MessageBox.Button.OK,
					Message = "Unexpected error: " + ex
				});
			}
		}

		private void SaveChanges()
		{
			zeusItemEditView.Save(zeusItemEditView.CurrentItem);
			ContentItem currentItem = zeusItemEditView.CurrentItem;

			if (currentItem.IsPage)
			{
				Refresh(currentItem, AdminFrame.Both);
				Title = string.Format("'{0}' saved, redirecting...", currentItem.Title);
				zeusItemEditView.Visible = false;
			}
			else
			{
				ExtNet.MessageBox.Show(new MessageBoxConfig
				{
					Icon = MessageBox.Icon.INFO,
					Buttons = MessageBox.Button.OK,
					Message = "Item saved"
				});
			}
		}

		/// <summary>Gets the type defined by <see cref="TypeDefinition"/>.</summary>
		/// <returns>The item's type.</returns>
		private Type CurrentItemType
		{
			get
			{
				ITypeDefinition contentType = TypeDefinition;
				if (contentType != null)
					return contentType.ItemType;
				return null;
			}
		}

		protected void zeusItemEditView_ItemCreating(object sender, ItemViewEditableObjectEventArgs e)
		{
			if (!string.IsNullOrEmpty(Discriminator))
			{
				ContentItem parentItem = Zeus.Context.Current.Resolve<Navigator>().Navigate(SelectedItem.Path);
				ContentItem contentItem = Zeus.Context.Current.ContentTypes.CreateInstance(CurrentItemType, parentItem);
				e.AffectedItem = contentItem;
			}
		}

		private ContentType TypeDefinition
		{
			get
			{
				if (!string.IsNullOrEmpty(Discriminator))
					return Zeus.Context.Current.ContentTypes.GetContentType(Discriminator);
				if (SelectedItem != null)
					return Zeus.Context.Current.ContentTypes.GetContentType(SelectedItem);
				return null;
			}
		}

		protected void zeusItemEditView_DefinitionCreating(object sender, ItemViewTypeDefinitionEventArgs e)
		{
			e.TypeDefinition = TypeDefinition;
		}

		protected override void OnPreRender(EventArgs e)
		{
			// The following resources are registered here because we can't register them during an Ext.NET AJAX request,
			// which means if the control wasn't already present on the page, the scripts will be missing.

			// FancyFileUpload
			ExtNet.ResourceManager.RegisterClientStyleInclude(typeof(EditorsResources),
				"Zeus.Editors.Resources.FancyFileUpload.FancyFileUpload.css");

			ExtNet.ResourceManager.RegisterIcon(Icon.Delete);
			ExtNet.ResourceManager.RegisterIcon(Icon.ArrowNsew);
			ExtNet.ResourceManager.RegisterClientScriptInclude(typeof(EditorsResources), "Zeus.Editors.Resources.FancyFileUpload.mootools.js");
			ExtNet.ResourceManager.RegisterClientScriptInclude(typeof(EditorsResources), "Zeus.Editors.Resources.FancyFileUpload.Fx.ProgressBar.js");
			ExtNet.ResourceManager.RegisterClientScriptInclude(typeof(EditorsResources), "Zeus.Editors.Resources.FancyFileUpload.Swiff.Uploader.js");
			ExtNet.ResourceManager.RegisterClientScriptInclude(typeof(EditorsResources), "Zeus.Editors.Resources.FancyFileUpload.FancyUpload3.Attach2.js");

			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.edit.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.view.css");
			base.OnPreRender(e);
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect(CancelUrl());
		}
	}
}