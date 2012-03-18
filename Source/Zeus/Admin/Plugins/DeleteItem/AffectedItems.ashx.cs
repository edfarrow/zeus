﻿using System;
using System.Linq;
using System.Web;
using Ext.Net;
using MongoDB.Bson;
using Zeus.Admin.Plugins.Tree;
using Zeus.Linq;
using Zeus.Security;

namespace Zeus.Admin.Plugins.DeleteItem
{
	public class AffectedItems : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/json";

			// "node" will be a comma-separated list of nodes that are going to be deleted.
			string node = context.Request["node"];

			if (!string.IsNullOrEmpty(node))
			{
				string[] nodeIDsTemp = node.Split(',');
				var nodeIDs = nodeIDsTemp.Select(s => ObjectId.Parse(s));

				TreeNodeCollection treeNodes = new TreeNodeCollection();

				foreach (ObjectId nodeID in nodeIDs)
				{
					ContentItem selectedItem = ContentItem.Find(nodeID);

					SiteTree tree = SiteTree.From(selectedItem);

					TreeNodeBase treeNode = tree.Filter(items => items.Authorized(context.User, Context.SecurityManager, Operations.Read))
						.ToTreeNode(false);

					treeNodes.Add(treeNode);
				}

				string json = treeNodes.ToJson();
				context.Response.Write(json);
				context.Response.End();
			}
		}

		public bool IsReusable
		{
			get { return false; }
		}
	}
}