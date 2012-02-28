using Ext.Net;
using Zeus.ContentTypes;
using Zeus.Integrity;

namespace Zeus
{
	[ContentType("Root Item", Installer = Installation.InstallerHints.PreferredRootPage)]
	[RestrictParents(AllowedTypes.None)]
	public class RootItem : DataContentItem, IRootItem
	{
		protected override Icon Icon
		{
			get { return Icon.PageGear; }
		}
	}
}