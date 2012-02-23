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

		public virtual Address ShippingAddress
		{
			get { return GetChild("shipping-address") as Address; }
			set
			{
				if (value != null)
				{
					value.Name = "shipping-address";
					value.Parent = this;
				}
			}
		}

        public virtual Address BillingAddress
		{
			get { return GetChild("billing-address") as Address; }
			set
			{
				if (value != null)
				{
					value.Name = "billing-address";
					value.Parent = this;
				}
			}
		}

        public virtual PaymentCard PaymentCard
		{
			get { return GetChild("payment-card") as PaymentCard; }
			set
			{
				if (value != null)
				{
					value.Name = "payment-card";
					value.Parent = this;
				}
			}
		}

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