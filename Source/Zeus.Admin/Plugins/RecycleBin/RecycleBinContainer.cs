using System.Linq;
using Ext.Net;
using MongoDB.Bson.Serialization.Attributes;
using Zeus.Admin.RecycleBin;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Installation;
using Zeus.Integrity;

namespace Zeus.Admin.Plugins.RecycleBin
{
	[ContentType("Recycle Bin", "RecycleBinContainer", Installer = InstallerHints.NeverRootOrStartPage)]
	[AllowedChildren(typeof(ContentItem))]
	[ContentTypeAuthorizedRoles(Roles = new string[0])]
	[RestrictParents(typeof(IRootItem))]
	[NotThrowable]
	public class RecycleBinContainer : ContentItem, IRecycleBin
	{
		[CheckBoxEditor("Enabled", "", 80)]
		[BsonDefaultValue(true)]
		public virtual bool Enabled { get; set; }

		[TextBoxEditor("Number of days to keep deleted items", 100, 10)]
		[BsonDefaultValue(31)]
		public virtual int KeepDays { get; set; }

		protected override Icon Icon
		{
			get { return Children.Any() ? Icon.Bin : Icon.BinClosed; }
		}

		public override bool IsPage
		{
			get { return false; }
		}
	}
}