using System.Web.Mvc;
using Zeus.Templates.Mvc.Controllers;
using Zeus.Web;
using Zeus.Examples.MinimalMvcExample.ContentTypes;
using Zeus.Examples.MinimalMvcExample.ViewModels;

namespace Zeus.Examples.MinimalMvcExample.Controllers
{
	[Controls(typeof(MyShop), AreaName = WebsiteAreaRegistration.AREA_NAME)]
	public class MyShopController : ZeusController<MyShop>
	{
		public override ActionResult Index()
		{
			return View(new MyShopViewModel(CurrentItem));
		}
	}
}
