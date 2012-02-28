using System.Linq;
using Ninject;
using Zeus.BaseLibrary.Reflection;
using Zeus.Plugin;

namespace Zeus.Admin.Plugins.Tree
{
	[AutoInitialize]
	public class TreePluginInitializer : IPluginInitializer
	{
		public void Initialize(IKernel kernel)
		{
			kernel.Get<ITypeFinder>().Find(typeof(ITreePlugin))
				.Where(t => !t.IsInterface && !t.IsAbstract)
				.ToList()
				.ForEach(t => kernel.Bind<ITreePlugin>().To(t));
		}
	}
}