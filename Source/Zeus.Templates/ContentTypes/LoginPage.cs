using Ext.Net;
using Zeus.Editors.Attributes;
using Zeus.Web;
using Zeus.Integrity;
using Zeus.Web.Security;

namespace Zeus.Templates.ContentTypes
{
    [ContentType("Login Page", "TemplatesLoginPage")]
    [RestrictParents(typeof(ILoginContext))]
	public class LoginPage : PageContentItem
	{
		protected override Icon Icon
		{
			get { return Icon.Key; }
		}

		[LinkedItemDropDownListEditor("Forgotten Password Page", 100)]
		public virtual PageContentItem ForgottenPasswordPage { get; set; }

		[LinkedItemDropDownListEditor("Registration Page", 110)]
		public virtual PageContentItem RegistrationPage { get; set; }
	}
}