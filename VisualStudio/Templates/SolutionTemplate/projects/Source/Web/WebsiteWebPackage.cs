using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Zeus.Web.Mvc.Modules;

namespace $safesolutionname$.Web
{
	public class WebsiteWebPackage : WebPackageBase
	{
		public const string AREA_NAME = "Website";

		public override void Register(IKernel container, ICollection<RouteBase> routes, ICollection<IViewEngine> viewEngines)
		{
			RegisterStandardArea(container, routes, viewEngines, AREA_NAME);
		}
	}
}