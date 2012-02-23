using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Ormongo;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.BaseLibrary.Collections.Generic;
using Zeus.Web;

namespace Zeus.AddIns.ECommerce.Services
{
	public class ShoppingBasketService : Observer<ContentItem>, IShoppingBasketService, IStartable
	{
		private readonly IWebContext _webContext;

		public ShoppingBasketService(IWebContext webContext)
		{
			_webContext = webContext;
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
				item.Parent = shoppingBasket;
			}
			else
			{
				item.Quantity += 1;
			}

			shoppingBasket.Save();
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

        	shoppingBasket.Save();
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
            GetCurrentShoppingBasketInternal(shop, true).Save();
        }

        #region IStartable Members

        public void Start()
        {
			ContentItem.Observers.Add(this);
        }

        public void Stop()
        {
        	ContentItem.Observers.Remove(this);
        }

		public override bool BeforeSave(ContentItem document)
		{
			CalculateBasketTotal(document);
			return base.BeforeSave(document);
		}

        #endregion

        /// <summary>
        /// Calculate basket total - default behaviour
        /// </summary>
        protected virtual void CalculateBasketTotal(ContentItem document)
        {
			if (document is IShoppingBasket)
            {
				var basket = document as IShoppingBasket;

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
			return ContentItem.Find<ShoppingBasket>(sb => sb.Name == shopperID);
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
                    shoppingBasket.Parent = shop.ShoppingBaskets;
                    shoppingBasket.Save();

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