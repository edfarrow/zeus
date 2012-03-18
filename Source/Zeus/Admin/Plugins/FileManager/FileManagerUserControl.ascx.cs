﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ext.Net;
using MongoDB.Bson;
using Zeus.Admin.Plugins.Tree;
using Zeus.Linq;
using Zeus.Security;

namespace Zeus.Admin.Plugins.FileManager
{
	[DirectMethodProxyID(Alias = "FileManager", IDMode = DirectMethodProxyIDMode.Alias)]
	public partial class FileManagerUserControl : PluginUserControlBase
	{
		public FileManagerUserControl()
		{
			ID = "fileManager";
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			// Data loader.
			var treeloader = new Ext.Net.TreeLoader
				{
					DataUrl =
						Engine.GetServerResourceUrl(typeof(FileManagerUserControl).Assembly,
							"Zeus.Admin.Plugins.FileManager.TreeLoader.ashx"),
					PreloadChildren = true
				};
			treePanel.Loader.Add(treeloader);

			filesStore.DirectEventConfig.Url = PluginBase.GetAdminDefaultUrl();

			if (ExtNet.IsAjaxRequest)
				return;

			ContentItem fileManagerRootFolder = Zeus.Context.StartPage;
			if (fileManagerRootFolder == null)
				return;

			TreeNodeBase treeNode = SiteTree.Between(fileManagerRootFolder, fileManagerRootFolder, true)
				.OpenTo(fileManagerRootFolder)
				.Filter(items => items.Authorized(Engine.WebContext.User, Engine.SecurityManager, Operations.Read))
				.ToTreeNode(true);
			treePanel.Root.Add(treeNode);

			filesStore.DataSource = GetFiles(fileManagerRootFolder, FileType.Both);
			filesStore.DataBind();
		}

		private static IEnumerable GetFiles(ContentItem contentItem, FileType fileType)
		{
			IEnumerable<ContentItem> files = contentItem.Children.Accessible().Pages().Published();
			switch (fileType)
			{
				case FileType.Image :
					files = files.Where(f => f is FileSystem.Images.Image);
					break;
			}
			return files.Select(ci => new
				{
					name = ci.Title,
					url = GetUrl(fileType, ci),
					imageUrl = GetImageUrl(ci)
				});
		}

		private static string GetUrl(FileType fileType, ContentItem ci)
		{
			if (fileType == FileType.Image)
				return ci.Url;
			return "~/link/" + ci.ID;
		}

		private static string GetImageUrl(ContentItem ci)
		{
			if (ci is FileSystem.Images.Image)
				return ((FileSystem.Images.Image) ci).GetUrl(80, 60, true);
			return ci.IconUrl;
		}

		protected void filesStore_RefreshData(object sender, StoreRefreshDataEventArgs e)
		{
			ObjectId nodeID = ObjectId.Parse(e.Parameters["node"]);
			FileType type = e.Parameters.Any(p => p.Name == "type") ? (FileType) Enum.Parse(typeof(FileType), e.Parameters["type"], true) : FileType.Both;
			ContentItem contentItem = ContentItem.Find(nodeID);
			filesStore.DataSource = GetFiles(contentItem, type);
			filesStore.DataBind();
		}

		private enum FileType
		{
			File,
			Image,
			Both
		}
	}
}