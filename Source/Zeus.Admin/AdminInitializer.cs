using Camino;
using Ninject;

namespace Zeus.Admin
{
	public class AdminInitializer : IInitializable
	{
		private readonly EmbeddedResourcePathProvider _embeddedResourcePathProvider;
		private readonly IAdminManager _adminManager;

		public AdminInitializer(EmbeddedResourcePathProvider embeddedResourcePathProvider, IAdminManager adminManager)
		{
			_embeddedResourcePathProvider = embeddedResourcePathProvider;
			_adminManager = adminManager;
		}

		public void Initialize()
		{
			_embeddedResourcePathProvider.AddAssembly(GetType().Assembly, _adminManager.AdminPath);
		}
	}
}