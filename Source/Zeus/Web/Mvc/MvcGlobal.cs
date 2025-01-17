using System;
using System.Web.Mvc;
using System.Web.Routing;
using Spark.Web.Mvc;
using Spark.Web.Mvc.Descriptors;
using Zeus.Configuration;
using Zeus.Engine;
using Zeus.Web.Mvc.Descriptors;

namespace Zeus.Web.Mvc
{
    public class MvcGlobal : Global
    {
        public override void Init()
        {
            // normally the engine is initialized by the initializer module but it can also be initialized this programmatically
            // since we attach programmatically we need to associate the event broker with a http application
            EventBroker.Instance.Attach(this);

            base.Init();
        }

        protected override void OnApplicationStart(EventArgs e)
        {
            base.OnApplicationStart(e);
            
            // Create and initialize Zeus engine.
            ContentEngine engine = Zeus.Context.Initialize(false);

            ViewEngines.Engines.Clear();

            // Create Spark view engine and register it with MVC.
            var sparkServiceContainer = SparkEngineStarter.CreateContainer();
            //sparkServiceContainer.AddFilter(new MobileDeviceDescriptorFilter());
            SparkEngineStarter.RegisterViewEngine(ViewEngines.Engines,
                sparkServiceContainer);

            ViewEngines.Engines.Add(new ViewEngine.ExtendedRazorViewEngine());

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
    }
}