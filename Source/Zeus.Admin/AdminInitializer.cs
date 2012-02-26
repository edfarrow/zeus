using Ninject;
using Zeus.Web.Hosting;

namespace Zeus.Admin
{
	public class AdminInitializer : IInitializable
	{
		private readonly IEmbeddedResourceManager _embeddedResourceManager;
		private readonly IAdminManager _adminManager;

		public AdminInitializer(IEmbeddedResourceManager embeddedResourceManager, IAdminManager adminManager)
		{
			_embeddedResourceManager = embeddedResourceManager;
			_adminManager = adminManager;
		}

		public void Initialize()
		{
			_embeddedResourceManager.AddAssembly(GetType().Assembly, _adminManager.AdminPath);
		}
	}
}