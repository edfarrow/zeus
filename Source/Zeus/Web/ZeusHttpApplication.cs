using System;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using Camino;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Spark.Web.Mvc;
using Zeus.Configuration;
using Zeus.Engine;
using Zeus.Web.Mvc;

[assembly: PreApplicationStartMethod(typeof(Zeus.Web.ZeusHttpApplication), "RegisterModules")]

namespace Zeus.Web
{
	public class ZeusHttpApplication : HttpApplication
	{
		protected void Application_Start(object sender, EventArgs e)
		{
			OnApplicationStart(e);
		}

		private static void RegisterRoutes(RouteCollection routes, ContentEngine engine)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.IgnoreRoute("{*extaxd}", new { extaxd = @"(.*/)?ext.axd(/.*)?" });

			string adminPath = Zeus.Context.Current.Resolve<AdminSection>().Path;
			routes.IgnoreRoute(adminPath + "/{*pathInfo}");
			routes.IgnoreRoute("assets" + "/{*pathInfo}");

			// This route detects content item paths and executes their controller
			routes.Add(new ContentRoute(engine));
		}

		private static void RegisterFallbackRoute(RouteCollection routes)
		{
			// This controller fallbacks to a controller unrelated to Zeus
			routes.MapRoute(
				"Default",                                              // Route name
				"{controller}/{action}/{id}",                           // URL with parameters
				new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
				);
		}

		public override void Init()
		{
			EventBroker.Instance.Attach(this);
			base.Init();
		}

		protected virtual void OnApplicationStart(EventArgs e)
		{
			// Create and initialize Zeus engine.
			ContentEngine engine = Zeus.Context.Initialize(false);

			// Create Spark view engine and register it with MVC.
			var sparkServiceContainer = SparkEngineStarter.CreateContainer();
			//sparkServiceContainer.AddFilter(new MobileDeviceDescriptorFilter());
			SparkEngineStarter.RegisterViewEngine(ViewEngines.Engines,
				sparkServiceContainer);

			// Register the primary routes used by Zeus.
			RegisterRoutes(RouteTable.Routes, engine);

			// Register areas (blogs, forums, etc). Mostly used for static assets
			// such as CSS and JS files. This needs to happen after Zeus has registered
			// content routes, because some areas might contain catch-all paths.
			AreaRegistration.RegisterAllAreas();

			// Set the controller factory to a custom one which uses the NinjectActionInvoker.
			ControllerBuilder.Current.SetControllerFactory(engine.Resolve<IControllerFactory>());

			// This must be the last route to be registered.
			RegisterFallbackRoute(RouteTable.Routes);

			// Use a custom model metadata provider.
			ModelMetadataProviders.Current = new CustomDataAnnotationsModelMetadataProvider();

			var virtualPathProvider = Zeus.Context.Current.Resolve<EmbeddedResourcePathProvider>();
			HostingEnvironment.RegisterVirtualPathProvider(virtualPathProvider);
		}

		public static void RegisterModules()
		{
			DynamicModuleUtility.RegisterModule(typeof(DefaultDocumentModule));
		}
	}
}