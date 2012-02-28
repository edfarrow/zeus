using Ext.Net;

namespace Zeus.Admin.Plugins
{
	public abstract class TreePluginBase : PluginBase, ITreePlugin
	{
		public virtual string[] RequiredScripts
		{
			get { return null; }
		}

		public virtual string[] RequiredUserControls
		{
			get { return null; }
		}

		public abstract void ModifyTree(TreePanel treePanel, IMainInterface mainInterface);

		public virtual void ModifyTreeNode(TreeNodeBase treeNode, ContentItem contentItem)
		{
			
		}
	}
}