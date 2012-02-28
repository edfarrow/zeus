using Ninject;
using Zeus.Plugin;

namespace Zeus.FileSystem.Images
{
	[AutoInitialize]
	public class ImageCachingServiceInitializer : IPluginInitializer
	{
		public void Initialize(IKernel kernel)
		{
			kernel.Bind<ImageCachingService>().ToSelf();
		}
	}
}