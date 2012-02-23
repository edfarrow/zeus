﻿using System;
using System.Linq;
using System.Web.UI.WebControls;
using MongoDB.Bson;
using Zeus.BaseLibrary.ExtensionMethods.Web.UI;
using Zeus.Integrity;

namespace Zeus.Admin.RecycleBin
{
	public partial class Default : PreviewFrameAdminPage
	{
		#region Properties

		protected IRecycleBinHandler RecycleBin
		{
			get { return Zeus.Context.Current.Resolve<IRecycleBinHandler>(); }
		}

		#endregion

		#region Methods

		protected override void OnLoad(EventArgs e)
		{
			hlCancel.NavigateUrl = CancelUrl();
			if (!IsPostBack)
				ReBind();
			cvRestore.IsValid = true;
			btnEmpty.Enabled = SelectedItem.GetChildren().Any();

			base.OnLoad(e);
		}

		protected void btnEmpty_Click(object sender, EventArgs e)
		{
			foreach (ContentItem child in SelectedItem.GetChildren())
				child.Destroy();
			ReBind();

			Refresh(SelectedItem, AdminFrame.Navigation, false);
		}

		protected void grvRecycleBinItems_RowCommand(object sender, GridViewCommandEventArgs e)
		{
			ObjectId itemID = ObjectId.Parse(e.CommandArgument.ToString());
			ContentItem item = Zeus.Context.Persister.Get(itemID);

			switch (e.CommandName)
			{
				case "Restore" :
					try
					{
						RecycleBin.Restore(item);
						ReBind();
					}
					catch (NameOccupiedException)
					{
						cvRestore.IsValid = false;
					}
					Refresh(item, AdminFrame.Navigation, false);
					break;
				case "Delete" :
					item.Destroy();
					ReBind();
					Refresh(SelectedItem, AdminFrame.Navigation, false);
					break;
			}
		}

		protected override void OnPreRender(EventArgs e)
		{
			Page.ClientScript.RegisterCssResource(typeof(Default), "Zeus.Admin.Assets.Css.view.css");
			base.OnPreRender(e);
		}

		private void ReBind()
		{
			grvRecycleBinItems.DataSource = SelectedItem.GetChildren();
			grvRecycleBinItems.DataBind();
		}

		#endregion

		protected void grvRecycleBinItems_RowDeleting(object sender, GridViewDeleteEventArgs e)
		{
			
		}
	}
}
