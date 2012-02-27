using System;
using System.Linq;
using Ext.Net;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.Admin;

namespace Zeus.AddIns.ECommerce.Admin.Plugins.ManageOrders
{
	public partial class Default : PreviewFrameAdminPage
	{
		protected override void OnLoad(EventArgs e)
		{
			if (!ExtNet.IsAjaxRequest)
				RefreshData();

			base.OnLoad(e);
		}

		protected void exsDataStore_RefreshData(object sender, StoreRefreshDataEventArgs e)
		{
			RefreshData();
		}

		private void RefreshData()
		{
			// TODO: Filtering

			exsDataStore.DataSource = SelectedItem.GetChildren<Order>().Where(o => o.Status == OrderStatus.Paid).OrderByDescending(o => o.ID).ToList();
			exsDataStore.DataBind();
		}

        protected void btnSeeAll_Click(object sender, EventArgs e)
		{
            btnSeeAll.Disabled = true;
			btnSeeOnlyUnprocessed.Disabled = false;

			exsDataStore.DataSource = SelectedItem.GetChildren<Order>().Where(o => o.Status != OrderStatus.Unpaid).OrderByDescending(o => o.ID).ToList();
			exsDataStore.DataBind();
        }

        protected void btnSeeOnlyUnprocessed_Click(object sender, EventArgs e)
        {
			btnSeeAll.Disabled = false;
			btnSeeOnlyUnprocessed.Disabled = true;

			RefreshData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
		{
            Response.Redirect(Engine.GetServerResourceUrl(typeof(Default), "Zeus.AddIns.ECommerce.Admin.Plugins.ManageOrders.Search.aspx") + "?selected=" + Request.QueryString["selected"]);
		}
	}
}