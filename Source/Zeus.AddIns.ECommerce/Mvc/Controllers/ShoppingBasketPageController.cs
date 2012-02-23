using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.Mvc.ViewModels;
using Zeus.AddIns.ECommerce.Services;
using Zeus.Templates.Mvc.Controllers;
using System.Web.Mvc;
using Zeus.Web;
using Zeus.Web.Mvc;

namespace Zeus.AddIns.ECommerce.Mvc.Controllers
{
	[Controls(typeof(ShoppingBasketPage), AreaName = ECommerceAreaRegistration.AREA_NAME)]
	public class ShoppingBasketPageController : ZeusController<ShoppingBasketPage>
	{
		private readonly IShoppingBasketService _shoppingBasketService;

		public ShoppingBasketPageController(IShoppingBasketService shoppingBasketService)
		{
			_shoppingBasketService = shoppingBasketService;
		}

		protected Shop CurrentShop
		{
			get { return (Shop) (CurrentItem.Parent); }
		}

		public override ActionResult Index()
		{
			IShoppingBasket shoppingBasket = GetShoppingBasket();
			IEnumerable<SelectListItem> deliveryMethods = null;
			if (CurrentShop.DeliveryMethods != null)
				deliveryMethods = CurrentShop.DeliveryMethods.GetChildren<DeliveryMethod>()
					.Select(dm => new SelectListItem
					{
						Text = dm.Title,
						Value = dm.ID.ToString(),
						Selected = (dm == shoppingBasket.DeliveryMethod)
					});
			return View(new ShoppingBasketPageViewModel(CurrentItem, GetShoppingBasket(), deliveryMethods, CurrentShop.CheckoutPage));
		}

		public ActionResult UpdateQuantity(
			[Bind(Prefix = "id")] ObjectId productID,
			[Bind(Prefix = "varid")] ObjectId? variationPermutationID,
			[Bind(Prefix = "qty")] int quantity)
		{
			Product product = ContentItem.Find<Product>(productID);
			VariationPermutation variationPermutation = (variationPermutationID != null) ? ContentItem.Find<VariationPermutation>(variationPermutationID.Value) : null;
			_shoppingBasketService.UpdateQuantity(CurrentShop, product, variationPermutation, quantity);
			
			return Redirect(CurrentItem.Url);
		}

		public ActionResult RemoveItem(
			[Bind(Prefix = "id")] ObjectId productID,
			[Bind(Prefix = "varid")] ObjectId? variationPermutationID)
		{
			return UpdateQuantity(productID, variationPermutationID, 0);
		}

		[ActionName("Checkout")]
		[AcceptVerbs(HttpVerbs.Post)]
		[AcceptImageSubmit(Name = "updateDeliveryMethod")]
		public ActionResult UpdateDeliveryMethod(ObjectId deliveryMethodID)
		{
			UpdateDeliveryMethodInternal(deliveryMethodID);
			return Redirect(CurrentItem.Url);
		}

		[AcceptVerbs(HttpVerbs.Post)]
		[AcceptImageSubmit(Name = "checkout")]
		public ActionResult Checkout(ObjectId deliveryMethodID)
		{
			UpdateDeliveryMethodInternal(deliveryMethodID);

			// Validate that there are items in the shopping basket.
			if (!GetShoppingBasket().Items.Any())
				return RedirectToParentPage();

			return Redirect(CurrentShop.CheckoutPage.Url);
		}

		private IShoppingBasket GetShoppingBasket()
		{
			return _shoppingBasketService.GetBasket(CurrentShop);
		}

		private void UpdateDeliveryMethodInternal(ObjectId deliveryMethodID)
		{
			IShoppingBasket shoppingBasket = GetShoppingBasket();
			shoppingBasket.DeliveryMethod = ContentItem.Find<DeliveryMethod>(deliveryMethodID);
			_shoppingBasketService.SaveBasket(CurrentShop);
		}
	}
}