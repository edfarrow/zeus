using System.Linq;
using Ext.Net;
using MongoDB.Bson.Serialization.Attributes;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Installation;
using Zeus.Integrity;
using Zeus.Web.Hosting;

namespace Zeus.Admin.RecycleBin
{
	[ContentType("Recycle Bin", "RecycleBinContainer", Installer = InstallerHints.NeverRootOrStartPage)]
	[AllowedChildren(typeof(ContentItem))]
	[ContentTypeAuthorizedRoles(Roles = new string[0])]
	[RestrictParents(typeof(IRootItem))]
	[NotThrowable]
	public class RecycleBinContainer : ContentItem, INode, IRecycleBin
	{
		[CheckBoxEditor("Enabled", "", 80)]
		[BsonDefaultValue(true)]
		public virtual bool Enabled { get; set; }

		[TextBoxEditor("Number of days to keep deleted items", 100, 10)]
		[BsonDefaultValue(31)]
		public virtual int KeepDays { get; set; }

		string INode.PreviewUrl
		{
			get { return Context.Current.Resolve<IEmbeddedResourceManager>().GetServerResourceUrl(typeof(RecycleBinContainer).Assembly, "Zeus.Admin.RecycleBin.Default.aspx") + "?selected=" + Path; }
		}

		protected override Icon Icon
		{
			get { return Children.Any() ? Icon.Bin : Icon.BinClosed; }
		}
	}
}