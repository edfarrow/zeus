using System.Linq;
using Ext.Net;
using Zeus.Integrity;
using Zeus.Security;
using Zeus.Security.ContentTypes;

namespace Zeus.Web.Security.Items
{
	[ContentType("User Container")]
	[RestrictParents(typeof(SecurityContainer))]
	public class UserContainer : DataContentItem
	{
		public const string ContainerName = "users";
		public const string ContainerTitle = "Users";

		public UserContainer()
		{
			Name = ContainerName;
			Title = ContainerTitle;
		}

		protected override Icon Icon
		{
			get { return Icon.Group; }
		}
	}
}
