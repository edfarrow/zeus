using System.Linq;
using Ninject;
using Zeus.Admin.Plugins;
using Zeus.BaseLibrary.Reflection;
using Zeus.Plugin;

namespace Zeus.Admin
{
	[AutoInitialize]
	public class MainInterfacePluginInitializer : IPluginInitializer
	{
		public void Initialize(IKernel kernel)
		{
			kernel.Get<ITypeFinder>().Find(typeof(IMainInterfacePlugin))
				.Where(t => !t.IsInterface && !t.IsAbstract)
				.ToList()
				.ForEach(t => kernel.Bind<IMainInterfacePlugin>().To(t));
		}
	}
}