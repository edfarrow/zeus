using System.Web.Routing;
using Ninject;
using Zeus.Editors.Resources;
using Zeus.Web.Routing;

namespace Zeus.Editors.Controls
{
	public class HtmlTextBoxResourcesInitializer : IInitializable
	{
		public void Initialize()
		{
			var assembly = typeof(EditorsResources).Assembly;
			var routeHandler = new EmbeddedContentRouteHandler(assembly, assembly.GetName().Name + ".CKEditor");
			RouteTable.Routes.Add(new Route(HtmlTextBox.GetBasePath() + "/{*resource}", new RouteValueDictionary(), routeHandler));
		}
	}
}