using System;
using Ext.Net;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;
using Zeus.Editors.Resources;
using Zeus.Security;
using Zeus.Web;

namespace Zeus.Admin.Plugins.EditItem
{
	[ActionPluginGroup("NewEditDelete", 10)]
	[AvailableOperation(Operations.Create, "Create", 20)]
	[AvailableOperation(Operations.Change, "Change", 30)]
	public partial class Default : AdminPage
	{
		private string Discriminator
		{
			get { return Request.QueryString["discriminator"]; }
		}

		protected override void OnInit(EventArgs e)
		{
			if (Discriminator != null)
			{
				ContentItem parentItem = Engine.Resolve<Navigator>().Navigate(SelectedItem.Path);
				var selectedContentType = Engine.ContentTypes.GetContentType(Discriminator);
				itemEditor.CurrentItem = Engine.ContentTypes.CreateInstance(selectedContentType.ItemType, parentItem);
				Title = "New " + selectedContentType.Title;
			}
			else
			{
				itemEditor.CurrentItem = SelectedItem;
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
			bool wasUpdated = itemEditor.UpdateItem();
			var currentItem = (ContentItem) itemEditor.CurrentItem;
			if (wasUpdated || currentItem.IsNewRecord)
			{
				currentItem.Save();

				ContentItem theParent = currentItem.Parent;
				while (theParent.Parent != null) // TODO: Shouldn't this be theParent != null ?
				{
					//go up the tree updating - if a child has been changed, so effectively has the parent
					theParent.Save();
					theParent = theParent.Parent;
				}
			}

			if (currentItem.IsPage)
			{
				Refresh(currentItem, AdminFrame.Both);
				Title = string.Format("'{0}' saved, redirecting...", currentItem.Title);
				itemEditor.Visible = false;
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

		protected override void OnPreRender(EventArgs e)
		{
			// The following resources are registered here because we can't register them during an Ext.NET AJAX request,
			// which means if the control wasn't already present on the page, the scripts will be missing.

			// FancyFileUpload
			ExtNet.ResourceManager.RegisterClientStyleInclude(typeof(EditorsResources), "Zeus.Editors.Resources.FancyFileUpload.FancyFileUpload.css");

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