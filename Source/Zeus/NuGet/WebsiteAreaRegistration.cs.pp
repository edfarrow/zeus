using Zeus.Web.Mvc;

namespace $rootnamespace$
{
	public class WebsiteAreaRegistration : StandardAreaRegistration
	{
		public const string AREA_NAME = "Website";

		public override string AreaName
		{
			get { return AREA_NAME; }
		}
	}
}