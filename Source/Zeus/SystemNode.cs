using Ext.Net;
using Zeus.Integrity;

namespace Zeus
{
	[ContentType("System Node")]
	[RestrictParents(typeof(RootItem))]
	public class SystemNode : DataContentItem
	{
		public SystemNode()
		{
			Name = "system";
			Title = "System";
		}

		protected override Icon Icon
		{
			get { return Icon.Computer; }
		}
	}
}
