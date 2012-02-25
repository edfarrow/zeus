using System.Collections.Generic;
using System.Linq;
using Ext.Net;
using Zeus.AddIns.ECommerce.Services;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Shopping Basket")]
	[RestrictParents(typeof(ShoppingBasketContainer))]
	public class ShoppingBasket : BaseContentItem, IShoppingBasket
	{
		protected override Icon Icon
		{
			get { return Icon.BasketPut; }
		}

		public virtual Address ShippingAddress { get; set; }
		public virtual Address BillingAddress { get; set; }
		public virtual PaymentCard PaymentCard { get; set; }

		public virtual DeliveryMethod DeliveryMethod { get; set; }
		public virtual string EmailAddress { get; set; }
		public virtual string TelephoneNumber { get; set; }
		public virtual string MobileTelephoneNumber { get; set; }

        public virtual IEnumerable<IShoppingBasketItem> Items
		{
			get { return GetChildren<ShoppingBasketItem>(); }
		}

        public virtual int TotalItemCount
		{
			get { return Items.Where(p => !p.Product.OutOfStock).Sum(i => i.Quantity); }
		}

        public virtual decimal SubTotalPrice
		{
            get { return Items.Where(p => !p.Product.OutOfStock).Sum(i => i.Product.CurrentPrice * i.Quantity); }
		}

        public virtual decimal SubTotalPriceForVatCalculation
        {
            get { return Items.Where(p => !p.Product.OutOfStock && !p.Product.VatZeroRated).Sum(i => i.Product.CurrentPrice * i.Quantity); }
        }

		public virtual decimal TotalDeliveryPrice { get; set; }
		public virtual decimal TotalVatPrice { get; set; }
		public virtual decimal TotalPrice { get; set; }
	}
}