using Ninject.Modules;

namespace Zeus.Admin
{
	public class AdminModule : NinjectModule
	{
		public override void Load()
		{
			Bind<AdminInitializer>().ToSelf().InSingletonScope();
		}
	}
}