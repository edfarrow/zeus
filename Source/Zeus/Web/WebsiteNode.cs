using Ext.Net;
using Zeus.Design.Editors;
using Zeus.FileSystem;
using Zeus.Integrity;
using Zeus.Web.Security;

namespace Zeus.Web
{
	[ContentType("Website", "BaseStartPage", Installer = Installation.InstallerHints.PreferredStartPage)]
	[RestrictParents(typeof(RootItem))]
	public class WebsiteNode : PageContentItem, IFileSystemContainer, ILoginContext
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.PageWorld); }
		}

		[LinkedItemDropDownListEditor("404 Page", 25, TypeFilter = typeof(PageContentItem), Description = "This page will be used if a user requests a page that does not exist.")]
		public virtual PageContentItem PageNotFoundPage { get; set; }

		#region Implementation of ILoginContext

		[LinkedItemDropDownListEditor("Login Page", 30, TypeFilter = typeof(PageContentItem), Description = "This page will be used if a user requests a page that they do not have access to.")]
		public virtual PageContentItem LoginPage { get; set; }

		[LinkedItemDropDownListEditor("Forgotten Password Page", 40, TypeFilter = typeof(PageContentItem), Description = "This page will be pointed to from any 'Lost your details' links.")]
		public virtual PageContentItem ForgottenPasswordPage { get; set; }

		public string LoginUrl
		{
			get { return (LoginPage != null) ? LoginPage.Url : null; }
		}

		#endregion
	}
}