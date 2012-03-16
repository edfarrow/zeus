using System;
using System.Linq;
using Ext.Net;
using Zeus.Collections;
using System.Collections.Generic;
using Zeus.Linq;
using Zeus.Util;

namespace Zeus.Admin.Plugins.Tree
{
	public class SiteTree
	{
		private readonly HierarchyBuilder _treeBuilder;
		private Func<IEnumerable<ContentItem>, IEnumerable<ContentItem>> _filter;

		public SiteTree(HierarchyBuilder treeBuilder)
		{
			_treeBuilder = treeBuilder;
		}

		public static SiteTree From(ContentItem rootItem)
		{
			return new SiteTree(new TreeHierarchyBuilder(rootItem));
		}

		public static SiteTree From(ContentItem rootItem, int maxDepth)
		{
			return new SiteTree(new TreeHierarchyBuilder(rootItem, maxDepth));
		}

		public static SiteTree Between(ContentItem initialItem, ContentItem lastAncestor, bool appendAdditionalLevel)
		{
			return new SiteTree(new BranchHierarchyBuilder(initialItem, lastAncestor, appendAdditionalLevel));
		}

		public SiteTree Filter(Func<IEnumerable<ContentItem>, IEnumerable<ContentItem>> filter)
		{
			_filter = filter;
			return this;
		}

		public SiteTree OpenTo(ContentItem item)
		{
			var items = item.Ancestors;
			//return ClassProvider(c => (items.Contains(c) || c == item) ? "open" : string.Empty);
			return this;
		}

		public TreeNodeBase ToTreeNode(bool rootOnly)
		{
			return ToTreeNode(rootOnly, true);
		}

		public TreeNodeBase ToTreeNode(bool rootOnly, bool withLinks)
		{
			IHierarchyNavigator<ContentItem> navigator = new ItemHierarchyNavigator(_treeBuilder, _filter);
			TreeNodeBase rootNode = BuildNodesRecursive(navigator, rootOnly, withLinks, _filter);
			return rootNode;
		}

		private static TreeNodeBase BuildNodesRecursive(IHierarchyNavigator<ContentItem> navigator, bool rootOnly, bool withLinks,
			Func<IEnumerable<ContentItem>, IEnumerable<ContentItem>> filter)
		{
			ContentItem item = navigator.Current;

			var itemChildren = item.Children.Accessible();
			if (filter != null)
				itemChildren = filter(itemChildren);
			bool hasAsyncChildren = ((!navigator.Children.Any() && itemChildren.Any()) || rootOnly);
			TreeNodeBase node = (hasAsyncChildren) ? new AsyncTreeNode() as TreeNodeBase : new TreeNode();
			node.Text = ((INode) item).Contents;

			node.IconFile = item.IconUrl;
			node.IconCls = "zeus-tree-icon";
			node.Cls = "zeus-tree-node";
			node.NodeID = item.ID.ToString();
			if (withLinks)
			{
				// Allow plugin to set the href (it will be based on whatever is the default context menu plugin).
				foreach (ITreePlugin treePlugin in Context.Current.ResolveAll<ITreePlugin>())
					treePlugin.ModifyTreeNode(node, item);
			}

			if (!hasAsyncChildren)
			{
				// Allow for grouping of child items into folders.
				var folderGroups = navigator.Children
					.Select(ci => ci.Current.FolderPlacementGroup)
					.Where(s => s != null)
					.Distinct();

                int uniqueCount = 0;
				foreach (string folderGroup in folderGroups)
				{
                    uniqueCount += 100000;
					TreeNode folderNode = new TreeNode
					{
						Text = folderGroup,
						IconFile = IconUtility.GetIconUrl(Icon.FolderGo),
						Cls = "zeus-tree-node",
						Expanded = false,
						// TODO: Check why this was necessary
                        //NodeID = (-1 * (Int32.Parse(node.NodeID) + uniqueCount)).ToString()
						NodeID = node.NodeID + uniqueCount
					};

					((TreeNode) node).Nodes.Add(folderNode);
					foreach (IHierarchyNavigator<ContentItem> childNavigator in navigator.Children.Where(n => n.Current.FolderPlacementGroup == folderGroup))
					{
						TreeNodeBase childNode = BuildNodesRecursive(childNavigator, rootOnly, withLinks, filter);
						folderNode.Nodes.Add(childNode);
					}
				}
				foreach (IHierarchyNavigator<ContentItem> childNavigator in navigator.Children.Where(n => n.Current.FolderPlacementGroup == null))
				{
					TreeNodeBase childNode = BuildNodesRecursive(childNavigator, rootOnly, withLinks, filter);
					((TreeNode) node).Nodes.Add(childNode);
				}
			}
			if (!itemChildren.Any())
			{
				node.CustomAttributes.Add(new ConfigItem("children", "[]", ParameterMode.Raw));
				node.Expanded = true;
			}
			else if (navigator.Children.Any())
				node.Expanded = true;
			return node;
		}
	}
}