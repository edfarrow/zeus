using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Coolite.Ext.UX;
using Ext.Net;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;
using Zeus.BaseLibrary.Web;
using Zeus.BaseLibrary.Web.UI;
using Zeus.Configuration;
using Zeus.ContentTypes;
using Zeus.Globalization;
using Zeus.Globalization.ContentTypes;
using Zeus.Integrity;
using Zeus.Security;
using Zeus.Web;
using Zeus.Web.Hosting;
using Zeus.Web.UI.WebControls;
using TreeNode = Ext.Net.TreeNode;

namespace Zeus.Admin.Plugins.EditItem
{
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
				if (SelectedLanguageCode != null)
				{
					ContentItem translatedItem = Engine.LanguageManager.GetTranslationDirect(SelectedItem, SelectedLanguageCode);
					if (translatedItem == null)
					{
						Title = string.Format("New Translation of '{0}'", SelectedItem.Title);
						ContentItem selectedItem = Engine.ContentTypes.CreateInstance(SelectedItem.GetType(), SelectedItem.Parent);
						selectedItem.Language = SelectedLanguageCode;
						selectedItem.TranslationOf = SelectedItem;
						SelectedItem.Translations.Add(selectedItem);
						selectedItem.Parent = null;
						zeusItemEditView.CurrentItem = selectedItem;
					}
					else
					{
						zeusItemEditView.CurrentItem = translatedItem;
						Title = "Edit \"" + translatedItem.Title + "\"";
					}
				}
				else
				{
					zeusItemEditView.CurrentItem = SelectedItem;
					Title = "Edit \"" + SelectedItem.Title + "\"";
				}
			}

			bool languagesVisible = GlobalizationEnabled && Engine.LanguageManager.CanBeTranslated((ContentItem) zeusItemEditView.CurrentItem);
			txiLanguages.Visible = ddlLanguages.Visible = languagesVisible;

			if (!ExtNet.IsAjaxRequest && GlobalizationEnabled)
			{
				foreach (Language language in Engine.Resolve<ILanguageManager>().GetAvailableLanguages())
				{
					IconComboListItem listItem = new IconComboListItem(language.Title, language.Name, language.IconUrl);
					ddlLanguages.Items.Add(listItem);
				}
			}

			base.OnInit(e);
		}

		protected string GetSessionKeepAliveUrl()
		{
			return Engine.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(
				typeof(KeepAlive).Assembly, "Zeus.Admin.Plugins.EditItem.KeepAlive.aspx");
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
				csvException.IsValid = false;
				csvException.ErrorMessage = ex.ToString();
			}
		}

		private void SaveChanges()
		{
			ContentItem currentItem = (ContentItem) zeusItemEditView.Save((ContentItem) zeusItemEditView.CurrentItem);

			if (Request["before"] != null)
			{
				ContentItem before = Engine.Resolve<Navigator>().Navigate(Request["before"]);
				Engine.Resolve<ITreeSorter>().MoveTo(currentItem, NodePosition.Before, before);
			}
			else if (Request["after"] != null)
			{
				ContentItem after = Engine.Resolve<Navigator>().Navigate(Request["after"]);
				Engine.Resolve<ITreeSorter>().MoveTo(currentItem, NodePosition.After, after);
			}

			Refresh(currentItem, AdminFrame.Both, false);
			Title = string.Format("'{0}' saved, redirecting...", currentItem.Title);
			zeusItemEditView.Visible = false;
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
				contentItem.Language = SelectedLanguageCode;
				if (contentItem is WidgetContentItem)
					((WidgetContentItem) contentItem).ZoneName = Page.Request["zoneName"];
				e.AffectedItem = contentItem;
			}
		}

		private ContentType TypeDefinition
		{
			get
			{
				if (!string.IsNullOrEmpty(Discriminator))
					return Zeus.Context.Current.ContentTypes[Discriminator];
				if (SelectedItem != null)
					return Zeus.Context.Current.ContentTypes[SelectedItem.GetType()];
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
			ExtNet.ResourceManager.RegisterClientStyleInclude(typeof(FancyFileUpload),
				"Zeus.Web.Resources.FancyFileUpload.FancyFileUpload.css");

			ExtNet.ResourceManager.RegisterIcon(Icon.Delete);
			ExtNet.ResourceManager.RegisterIcon(Icon.ArrowNsew);
			ExtNet.ResourceManager.RegisterClientScriptInclude(typeof(FancyFileUpload), "Zeus.Web.Resources.FancyFileUpload.mootools.js");
			ExtNet.ResourceManager.RegisterClientScriptInclude(typeof(FancyFileUpload), "Zeus.Web.Resources.FancyFileUpload.Fx.ProgressBar.js");
			ExtNet.ResourceManager.RegisterClientScriptInclude(typeof(FancyFileUpload), "Zeus.Web.Resources.FancyFileUpload.Swiff.Uploader.js");
			ExtNet.ResourceManager.RegisterClientScriptInclude(typeof(FancyFileUpload), "Zeus.Web.Resources.FancyFileUpload.FancyUpload3.Attach2.js");

			// HtmlTextBox
			Page.ClientScript.RegisterJavascriptInclude(Utility.GetClientResourceUrl(typeof(HtmlTextBox), "TinyMCE/tiny_mce.js"), ResourceInsertPosition.HeaderTop);
			Page.ClientScript.RegisterClientScriptBlock(typeof(HtmlTextBox), "HtmlTextBox",
				@"function fileBrowserCallBack(fieldName, url, destinationType, win)
				{
					var srcField = win.document.forms[0].elements[fieldName];
					var insertFileUrl = function(data) {
						srcField.value = data.url;
					};

					top.fileManager.show(Ext.get(fieldName), insertFileUrl, destinationType);
				}", true);

			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.edit.css");
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.view.css");
			ddlLanguages.SelectedItem.Value = SelectedLanguageCode;
			base.OnPreRender(e);
		}

		protected void ddlLanguages_ValueChanged(object sender, DirectEventArgs e)
		{
			ExtNet.Redirect(new Url(Request.RawUrl).SetQueryParameter("language", ddlLanguages.SelectedItem.Value));
		}

		protected void btnCancel_Click(object sender, EventArgs e)
		{
			Response.Redirect(CancelUrl());
		}
	}
}