using System;
using System.Linq;
using System.Web;
using Ext.Net;
using MongoDB.Bson;
using Zeus.Linq;
using Zeus.Security;
using Zeus.Web;

namespace Zeus.Admin.Plugins.Tree
{
	public class TreeLoader : IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			context.Response.ContentType = "text/json";
			string fromRootTemp = context.Request["fromRoot"];
			bool fromRoot = false;
			if (!string.IsNullOrEmpty(fromRootTemp))
				fromRoot = Convert.ToBoolean(fromRootTemp);
			bool sync = (context.Request["sync"] == "true");
			ObjectId? nodeId = !string.IsNullOrEmpty(context.Request["node"]) ? ObjectId.Parse(context.Request["node"]) as ObjectId? : null;
			ObjectId? overrideNodeId = !string.IsNullOrEmpty(context.Request["overrideNode"]) ? ObjectId.Parse(context.Request["overrideNode"]) as ObjectId? : null;

			nodeId = overrideNodeId ?? nodeId;
			if (nodeId != null)
			{
				ContentItem selectedItem = ContentItem.Find(nodeId.Value);

				SiteTree tree;
				if (sync)
					tree = SiteTree.From(selectedItem, int.MaxValue);
				else if (fromRoot)
					tree = SiteTree.Between(selectedItem, Find.RootItem, true)
						.OpenTo(selectedItem);
				else
					tree = SiteTree.From(selectedItem, 2);

				if (sync)
					tree = tree.ForceSync();

				//if (context.User.Identity.Name != "administrator")
				//	filter = new CompositeSpecification<ContentItem>(new PageSpecification<ContentItem>(), filter);
				TreeNodeBase treeNode = tree.Filter(items => items
					.Authorized(context.User, Context.SecurityManager, Operations.Read)
					.Where(TreeMainInterfacePlugin.IsVisibleInTree))
					.ToTreeNode(false);

				if (treeNode is TreeNode)
				{
					string json = ((TreeNode) treeNode).Nodes.ToJson();
					context.Response.Write(json);
					context.Response.End();
				}
			}
		}

		public bool IsReusable
		{
			get { return false; }
		}
	}
}