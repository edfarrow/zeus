using Ninject;
using Zeus.Configuration;
using Zeus.Plugin;

namespace Zeus.Admin.Plugins.RecycleBin
{
	[AutoInitialize]
	public class RecycleBinInitializer : IPluginInitializer
	{
		public void Initialize(IKernel kernel)
		{
			if (kernel.Get<AdminSection>().RecycleBin.Enabled)
			{
				kernel.Bind<IRecycleBinHandler>().To<RecycleBinHandler>();
				kernel.Bind<DeleteInterceptor>().ToSelf();
			}
		}
	}
}