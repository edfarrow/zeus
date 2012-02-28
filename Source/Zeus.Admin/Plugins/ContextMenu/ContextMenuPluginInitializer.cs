using System.Linq;
using Ninject;
using Zeus.BaseLibrary.Reflection;
using Zeus.Plugin;

namespace Zeus.Admin.Plugins.ContextMenu
{
	[AutoInitialize]
	public class ContextMenuPluginInitializer : IPluginInitializer
	{
		public void Initialize(IKernel kernel)
		{
			kernel.Get<ITypeFinder>().Find(typeof(IContextMenuPlugin))
				.Where(t => !t.IsInterface && !t.IsAbstract)
				.ToList()
				.ForEach(t => kernel.Bind<IContextMenuPlugin>().To(t));
		}
	}
}