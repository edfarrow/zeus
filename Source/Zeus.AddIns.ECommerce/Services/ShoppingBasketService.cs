using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.BaseLibrary.Collections.Generic;
using Zeus.Persistence;
using Zeus.Web;

namespace Zeus.AddIns.ECommerce.Services
{
	public class ShoppingBasketService : IShoppingBasketService, IStartable
	{
		private readonly IPersister _persister;
		private readonly IWebContext _webContext;
		private readonly IFinder _finder;

		public ShoppingBasketService(IPersister persister, IWebContext webContext, IFinder finder)
		{
			_persister = persister;
			_webContext = webContext;
			_finder = finder;
		}

        public virtual bool IsValidVariationPermutation(Product product, IEnumerable<Variation> variations)
		{
			if ((variations == null || !variations.Any()) && (product.VariationConfigurations == null || !product.VariationConfigurations.Any()))
				return true;

			return product.VariationConfigurations.Any(vc => vc.Available
				&& EnumerableUtility.EqualsIgnoringOrder(vc.Permutation.Variations.Cast<Variation>(), variations));
		}

        public virtual void AddItem(Shop shop, Product product, IEnumerable<Variation> variations)
		{
			ShoppingBasket shoppingBasket = GetCurrentShoppingBasketInternal(shop, true);

			// If card is already in basket, just increment quantity, otherwise create a new item.
			ShoppingBasketItem item = shoppingBasket.GetChildren<ShoppingBasketItem>().SingleOrDefault(i => i.Product == product && ((variations == null && i.Variations == null) || EnumerableUtility.Equals(i.Variations, variations)));
			if (item == null)
			{
				VariationPermutation variationPermutation = null;
				if (variations != null && variations.Any())
				{
					variationPermutation = new VariationPermutation();
					foreach (Variation variation in variations)
						variationPermutation.Variations.Add(variation);
				}
				item = new ShoppingBasketItem { Product = product, VariationPermutation = variationPermutation, Quantity = 1 };
				item.AddTo(shoppingBasket);
			}
			else
			{
				item.Quantity += 1;
			}

			_persister.Save(shoppingBasket);
		}

        public virtual void RemoveItem(Shop shop, Product product, VariationPermutation variationPermutation)
		{
			UpdateQuantity(shop, product, variationPermutation, 0);
		}

        public virtual void UpdateQuantity(Shop shop, Product product, VariationPermutation variationPermutation, int newQuantity)
		{
			if (newQuantity < 0)
				throw new ArgumentOutOfRangeException("newQuantity", "Quantity must be greater than or equal to 0.");

			ShoppingBasket shoppingBasket = GetCurrentShoppingBasketInternal(shop, true);
			ShoppingBasketItem item = shoppingBasket.GetChildren<ShoppingBasketItem>().SingleOrDefault(i => i.Product == product && i.VariationPermutation == variationPermutation);

			if (item == null)
				return;

        	if (newQuantity == 0)
        		item.Destroy();
        	else
        		item.Quantity = newQuantity;

        	_persister.Save(shoppingBasket);
		}

        public virtual ShoppingBasket GetBasket(Shop shop)
		{
			return GetCurrentShoppingBasketInternal(shop, false);
		}

        public virtual void ClearBasket(Shop shop)
		{
			ShoppingBasket shoppingBasket = GetCurrentShoppingBasketInternal(shop, false);
			if (shoppingBasket != null && !shoppingBasket.IsNewRecord)
			{
				shoppingBasket.Destroy();
				_webContext.Response.Cookies.Remove(GetCookieKey(shop));
			}
		}

        public virtual void SaveBasket(Shop shop)
        {
            _persister.Save(GetCurrentShoppingBasketInternal(shop, true));
        }

        #region IStartable Members

        public void Start()
        {
            _persister.ItemSaving += CalculateBasketTotal;
        }

        public void Stop()
        {
            _persister.ItemSaving -= CalculateBasketTotal;
        }

        #endregion

        /// <summary>
        /// Calculate basket total - default behaviour
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void CalculateBasketTotal(object sender, Zeus.CancelItemEventArgs e)
        {
            if (e.AffectedItem is IShoppingBasket)
            {
                var basket = e.AffectedItem as IShoppingBasket;

                // set delivery price
                if (basket.DeliveryMethod != null)
                    basket.TotalDeliveryPrice = basket.DeliveryMethod.Price;

                // set VAT price
                basket.TotalVatPrice = 0m;

                // set final total
                basket.TotalPrice = basket.SubTotalPrice + basket.TotalDeliveryPrice + basket.TotalVatPrice;
            }
        }

        private static string GetCookieKey(Shop shop)
		{
			return "ZeusECommerce" + shop.ID;
		}

		private ShoppingBasket GetShoppingBasketFromCookie(Shop shop)
		{
			HttpCookie cookie = _webContext.Request.Cookies[GetCookieKey(shop)];
			if (cookie == null)
				return null;

			string shopperID = cookie.Value;
			return _finder.QueryItems<ShoppingBasket>().SingleOrDefault(sb => sb.Name == shopperID);
		}

		private ShoppingBasket GetCurrentShoppingBasketInternal(Shop shop, bool createBasket)
		{
			ShoppingBasket shoppingBasket = GetShoppingBasketFromCookie(shop);

			if (shoppingBasket != null)
			{
				// Check that products in shopping cart still exist, and if not remove those shopping cart items.
				List<ShoppingBasketItem> itemsToRemove = new List<ShoppingBasketItem>();
				foreach (ShoppingBasketItem item in shoppingBasket.GetChildren<ShoppingBasketItem>())
					if (item.Product == null)
						itemsToRemove.Add(item);
				foreach (ShoppingBasketItem item in itemsToRemove)
					item.Destroy();
			}
			else
			{
				shoppingBasket = new ShoppingBasket { Name = Guid.NewGuid().ToString() };
				if (shop.DeliveryMethods != null)
					shoppingBasket.DeliveryMethod = shop.DeliveryMethods.GetChildren<DeliveryMethod>().FirstOrDefault();

                // don't always save basket to database
                // this will only happen when we add items to it
                if (createBasket)
                {
                    shoppingBasket.AddTo(shop.ShoppingBaskets);
                    _persister.Save(shoppingBasket);

                    HttpCookie cookie = new HttpCookie(GetCookieKey(shop), shoppingBasket.Name);
                    if (shop.PersistentShoppingBaskets)
                        cookie.Expires = DateTime.Now.AddYears(1);
                    _webContext.Response.Cookies.Add(cookie);
                }
			}

			return shoppingBasket;
		}        
    }
}