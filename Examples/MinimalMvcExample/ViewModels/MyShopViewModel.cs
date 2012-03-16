using Zeus.Web.Mvc.ViewModels;
using Zeus.Examples.MinimalMvcExample.ContentTypes;

namespace Zeus.Examples.MinimalMvcExample.ViewModels
{
	public class MyShopViewModel : ViewModel<MyShop>
	{
		public MyShopViewModel(MyShop currentItem)
			: base(currentItem)
		{
            
		}
	}
}
