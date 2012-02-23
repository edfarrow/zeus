using System;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;

namespace Zeus.Admin.Plugins.CopyItem
{
	public partial class Default : PreviewFrameAdminPage
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
				LoadDefaultsAndInfo();
		}

		private void LoadDefaultsAndInfo()
		{
			hlCancel.NavigateUrl = CancelUrl();
			txtNewName.Text = MemorizedItem.Name;

			Title = string.Format("Copy '{0}' onto '{1}'",
				MemorizedItem.Title,
				SelectedItem.Title);

			from.Text = string.Format("{0}<b>{1}</b>",
				GetBreadcrumbPath(MemorizedItem.Parent),
				MemorizedItem.Name);

			to.Text = string.Format("{0}<b>{1}</b>",
				GetBreadcrumbPath(SelectedItem),
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
				Refresh(newItem, AdminFrame.Both, false);
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
	}
}