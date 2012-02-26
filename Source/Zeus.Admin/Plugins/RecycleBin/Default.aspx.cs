using System;
using System.Linq;
using Ext.Net;
using MongoDB.Bson;
using Zeus.Integrity;
using Zeus.Linq;

namespace Zeus.Admin.Plugins.RecycleBin
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
			if (!ExtNet.IsAjaxRequest)
				RefreshData();

			cvRestore.IsValid = true;

			base.OnLoad(e);
		}

		protected void exsDataStore_RefreshData(object sender, StoreRefreshDataEventArgs e)
		{
			RefreshData();
		}

		private void RefreshData()
		{
			exsDataStore.DataSource = SelectedItem.Children.Accessible().Select(c => new
			{
				ID = c.ID.ToString(),
				c.Title,
				DeletedDate = c.ExtraData[RecycleBinHandler.DeletedDate],
				PreviousLocation = c.ExtraData[RecycleBinHandler.FormerParentTitle],
				c.IconUrl
			}).ToArray();
			exsDataStore.DataBind();

			btnEmpty.Enabled = SelectedItem.Children.Accessible().Any();
		}

		protected void btnEmpty_Click(object sender, DirectEventArgs e)
		{
			foreach (ContentItem child in SelectedItem.Children.Accessible())
				child.Destroy();
			RefreshData();
		}

		protected void gpaChildren_Command(object sender, DirectEventArgs e)
		{
			ObjectId itemID = ObjectId.Parse(e.ExtraParams["ID"]);
			ContentItem item = ContentItem.Find(itemID);

			switch (e.ExtraParams["CommandName"])
			{
				case "Restore" :
					try
					{
						RecycleBin.Restore(item);
						RefreshData();
					}
					catch (NameOccupiedException)
					{
						cvRestore.IsValid = false;
					}
					break;
				case "Delete" :
					item.Destroy();
					RefreshData();
					break;
			}
		}

		#endregion
	}
}
