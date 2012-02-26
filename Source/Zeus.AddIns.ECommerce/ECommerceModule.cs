using Ninject.Modules;
using Zeus.AddIns.ECommerce.Admin;
using Zeus.AddIns.ECommerce.Services;
using Zeus.Web.Hosting;

namespace Zeus.AddIns.ECommerce
{
	public class ECommerceModule : NinjectModule
	{
		public override void Load()
		{
			Bind<IShoppingBasketService>().To<ShoppingBasketService>().InSingletonScope();
			Bind<IOrderService>().To<OrderService>().InSingletonScope();
			Bind<AdminInitializer>().ToSelf().InSingletonScope();
		}
	}
}