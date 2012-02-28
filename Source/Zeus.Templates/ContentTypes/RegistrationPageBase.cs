using Ext.Net;
using Zeus.Integrity;
using Zeus.Web;
using Zeus.Web.Security;

namespace Zeus.Templates.ContentTypes
{
	[RestrictParents(typeof(ILoginContext))]
	public class RegistrationPageBase : PageContentItem
	{
        public string MonkeyNonStat { get { return "Monkey"; } }

		protected override Icon Icon
		{
			get { return Icon.KeyAdd; }
		}
	}
}