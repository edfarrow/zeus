using System.Collections.Generic;
using System.Linq;
using Ext.Net;
using Zeus.Integrity;
using Zeus.Security;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType]
	[RestrictParents(typeof(OrderContainer))]
	public class Order : BaseContentItem
	{
		protected override Icon Icon
		{
			get { return Icon.BasketPut; }
		}

		public User User { get; set; }

		public Address ShippingAddress
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

		public Address BillingAddress
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

		public PaymentCard PaymentCard
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

		public OrderStatus Status { get; set; }
		public PaymentMethod PaymentMethod { get; set; }
		public string BookingRef { get; set; }
		public DeliveryMethod DeliveryMethod { get; set; }
		public string EmailAddress { get; set; }
		public string TelephoneNumber { get; set; }
		public string MobileTelephoneNumber { get; set; }

		public IEnumerable<OrderItem> Items
		{
			get { return GetChildren<OrderItem>(); }
		}

		public int TotalItemCount
		{
			get { return Items.Sum(i => i.Quantity); }
		}

		public decimal SubTotalPrice
		{
			get { return Items.Sum(i => i.LineTotal); }
		}

		public decimal TotalDeliveryPrice { get; set; }
		public virtual decimal TotalVatPrice { get; set; }
		public decimal TotalPrice { get; set; }
	}
}