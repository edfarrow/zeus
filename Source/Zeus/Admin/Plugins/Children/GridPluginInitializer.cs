using System.Linq;
using Ninject;
using Zeus.BaseLibrary.Reflection;
using Zeus.Plugin;

namespace Zeus.Admin.Plugins.Children
{
	[AutoInitialize]
	public class GridPluginInitializer : IPluginInitializer
	{
		public void Initialize(IKernel kernel)
		{
			kernel.Get<ITypeFinder>().Find(typeof(IGridToolbarPlugin))
				.Where(t => !t.IsInterface && !t.IsAbstract)
				.ToList()
				.ForEach(t => kernel.Bind<IGridToolbarPlugin>().To(t));
		}
	}
}