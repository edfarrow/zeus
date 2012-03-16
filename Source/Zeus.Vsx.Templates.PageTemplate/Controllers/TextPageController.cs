using System.Web.Mvc;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;
using $rootnamespace$.ContentTypes;
using $rootnamespace$.ViewModels;

namespace $rootnamespace$.Controllers
{
	[Controls(typeof($safeitemname$), AreaName = WebsiteAreaRegistration.AREA_NAME)]
	public class $safeitemname$Controller : ZeusController<$safeitemname$>
	{
		public override ActionResult Index()
		{
			return View(new $safeitemname$ViewModel(CurrentItem));
		}
	}
}
