using Ninject;
using Zeus.Plugin;

namespace Zeus.Admin.NavigationFlag
{
	[AutoInitialize]
	public class NavigationCachingServiceInitializer : IPluginInitializer
	{
		public void Initialize(IKernel kernel)
		{
            kernel.Bind<NavigationCachingService>().ToSelf();
		}
	}
}