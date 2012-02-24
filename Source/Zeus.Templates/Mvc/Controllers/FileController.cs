using System.Web.Mvc;
using Zeus.FileSystem;
using Zeus.Web;

namespace Zeus.Templates.Mvc.Controllers
{
	[Controls(typeof(File), AreaName = TemplatesAreaRegistration.AREA_NAME)]
	public class FileController : ZeusController<File>
	{
		public override ActionResult Index()
		{
			return File(CurrentItem.Data.Data.Content, CurrentItem.Data.Data.ContentType);
		}
	}
}