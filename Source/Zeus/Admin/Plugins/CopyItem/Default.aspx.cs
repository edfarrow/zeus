using System;
using System.Web.UI.WebControls;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;

namespace Zeus.Admin.Plugins.CopyItem
{
	public partial class Default : AdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				LoadDefaultsAndInfo();
		}

		private void LoadDefaultsAndInfo()
		{
			txtNewName.Text = MemorizedItem.Name;

			Title = string.Format("Copy '{0}' onto '{1}'",
				MemorizedItem.Title,
				SelectedItem.Title);

			from.Text = string.Format("{0}<b>{1}</b>",
				MemorizedItem.Parent.Path,
				MemorizedItem.Name);

			to.Text = string.Format("{0}<b>{1}</b>",
				SelectedItem.Path,
				MemorizedItem.Name);

			itemsToCopy.CurrentItem = MemorizedItem;
			itemsToCopy.DataBind();
		}

		protected void btnCopy_Click(object sender, EventArgs e)
		{
			try
			{
				// Check user has permission to create items under the SelectedItem
				pnlNewName.Visible = false;
				ContentItem newItem = MemorizedItem.Clone();
				newItem.Name = txtNewName.Text;
				newItem = newItem.CopyTo(SelectedItem);
				Refresh(newItem, AdminFrame.Both);
			}
			catch (Integrity.NameOccupiedException ex)
			{
				pnlNewName.Visible = true;
				SetErrorMessage(cvCopy, ex);
			}
			catch (ContentTypes.NotAllowedParentException ex)
			{
				SetErrorMessage(cvCopy, ex);
			}
			catch (Exception ex)
			{
				SetErrorMessage(cvCopy, ex);
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.edit.css");
			base.OnPreRender(e);
		}

		private void SetErrorMessage(BaseValidator validator, Integrity.NameOccupiedException ex)
		{
			Trace.Write(ex.ToString());

			string message = string.Format("An item named '{0}' already exists below '{1}'",
				ex.SourceItem.Name,
				ex.DestinationItem.Name);
			SetErrorMessage(validator, message);
		}

		private void SetErrorMessage(BaseValidator validator, ContentTypes.NotAllowedParentException ex)
		{
			//Trace.Write(ex.ToString());

			string message = string.Format("The item of type '{0}' isn't allowed below a destination of type '{1}'",
				ex.ContentType.Title,
				Engine.ContentTypes.GetContentType(ex.ParentType).Title);
			SetErrorMessage(validator, message);
		}

		private static void SetErrorMessage(BaseValidator validator, Exception exception)
		{
			//Engine.Resolve<IErrorHandler>().Notify(exception);
			SetErrorMessage(validator, exception.Message);
		}

		private static void SetErrorMessage(BaseValidator validator, string message)
		{
			validator.IsValid = false;
			validator.ErrorMessage = message;
		}
	}
}