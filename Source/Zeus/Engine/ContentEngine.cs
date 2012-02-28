using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Ninject;
using Ormongo;
using Zeus.Admin;
using Zeus.BaseLibrary.Reflection;
using Zeus.BaseLibrary.Web;
using Zeus.Configuration;
using Zeus.ContentTypes;
using Zeus.Plugin;
using Zeus.Security;
using Zeus.Web;
using Zeus.Web.Hosting;
using Zeus.Web.Security;
using IWebContext=Zeus.Web.IWebContext;

namespace Zeus.Engine
{
	public class ContentEngine
	{
		private readonly ZeusKernel _kernel;

		#region Properties

		public IAdminManager AdminManager
		{
			get { return Resolve<IAdminManager>(); }
		}

		public IContentTypeManager ContentTypes
		{
			get { return Resolve<IContentTypeManager>(); }
		}

		public IEditableTypeManager EditableTypes
		{
			get { return Resolve<IEditableTypeManager>(); }
		}

		public ISecurityManager SecurityManager
		{
			get { return Resolve<ISecurityManager>(); }
		}

		public IWebSecurityService WebSecurity
		{
			get { return Resolve<IWebSecurityService>(); }
		}

		public IHost Host
		{
			get { return Resolve<IHost>(); }
		}

		public IUrlParser UrlParser
		{
			get { return Resolve<IUrlParser>(); }
		}

		public IWebContext WebContext
		{
			get { return Resolve<IWebContext>(); }
		}

		#endregion

		#region Constructor

		public ContentEngine(EventBroker eventBroker)
		{
			_kernel = new ZeusKernel();
			_kernel.Bind<IAssemblyFinder>().To<AssemblyFinder>();

			var hostSection = (HostSection)ConfigurationManager.GetSection("zeus/host");
			var databaseSection = (DatabaseSection)ConfigurationManager.GetSection("zeus/database");
			_kernel.Bind<EventBroker>().ToConstant(eventBroker);
			_kernel.Bind<DatabaseSection>().ToConstant(databaseSection);
			_kernel.Bind<HostSection>().ToConstant(hostSection);
			_kernel.Bind<AdminSection>().ToConstant(ConfigurationManager.GetSection("zeus/admin") as AdminSection);
			_kernel.Bind<ContentTypesSection>().ToConstant(ConfigurationManager.GetSection("zeus/contentTypes") as ContentTypesSection);
			_kernel.Bind<CustomUrlsSection>().ToConstant(ConfigurationManager.GetSection("zeus/customUrls") as CustomUrlsSection ?? new CustomUrlsSection());

			OrmongoConfiguration.Database = databaseSection.DatabaseName;
			OrmongoConfiguration.ServerHost = databaseSection.ServerHost;
			OrmongoConfiguration.ServerPort = databaseSection.ServerPort;

			if (hostSection != null && hostSection.Web != null)
				Url.DefaultExtension = hostSection.Web.Extension;
		}

		#endregion

		public void Initialize()
		{
			_kernel.Bind<ContentEngine>().ToConstant(this);

			var invoker = Resolve<IPluginBootstrapper>();
			invoker.InitializePlugins(_kernel, invoker.GetPluginDefinitions());

			_kernel.InitializeServices();
		}

		public T Resolve<T>()
		{
			return _kernel.Get<T>();
		}

		public IEnumerable<T> ResolveAll<T>()
		{
			return _kernel.GetAll<T>();
		}

		public object Resolve(Type serviceType)
		{
			return _kernel.Get(serviceType);
		}

		public string GetServerResourceUrl(Assembly assembly, string resourcePath)
		{
			return Resolve<IEmbeddedResourceManager>().GetUrl(assembly, resourcePath);
		}

		public string GetServerResourceUrl(Type type, string resourcePath)
		{
			return GetServerResourceUrl(type.Assembly, resourcePath);
		}
	}
}
