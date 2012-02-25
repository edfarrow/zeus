using System;
using System.Collections.Generic;
using System.Linq;
using Zeus.AddIns.ECommerce.ContentTypes;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.AddIns.ECommerce.PaymentGateways;
using Zeus.Web;
using Zeus.Web.Security;

namespace Zeus.AddIns.ECommerce.Services
{
	public class OrderService : IOrderService
	{
		private readonly IWebContext _webContext;
		private readonly IPaymentGateway _paymentGateway;

		public OrderService(IWebContext webContext, IPaymentGateway paymentGateway)
		{
			_webContext = webContext;
			_paymentGateway = paymentGateway;
		}

		/// <summary>
		/// Masks the credit card using XXXXXX and appends the last 4 digits
		/// </summary>
		public string GetMaskedCardNumber(string cardNumber)
		{
			string result = "****";
			if (cardNumber.Length > 8)
			{
				string lastFour = cardNumber.Substring(cardNumber.Length - 4, 4);
				result = "**** **** **** " + lastFour;
			}
			return result;
		}

		public IEnumerable<PaymentCardType> GetSupportedCardTypes()
		{
			return Enum.GetValues(typeof(PaymentCardType)).Cast<PaymentCardType>()
				.Where(paymentCardType => _paymentGateway.SupportsCardType(paymentCardType));
		}

		public Order PlaceOrder(IECommerceConfiguration configuration, string cardNumber, string cardVerificationCode,
			DeliveryMethod deliveryMethod, decimal deliveryPrice, Address billingAddress,
			Address shippingAddress, PaymentCard paymentCard, string emailAddress,
			string telephoneNumber, string mobileTelephoneNumber,
			IEnumerable<OrderItem> items,
            decimal totalVatPrice, decimal totalPrice)
		{
			// Convert shopping basket into order, with unpaid status.
			Order order = new Order
			{
				User = (_webContext.User != null && (_webContext.User is WebPrincipal)) ? ((WebPrincipal) _webContext.User).MembershipUser : null,
				DeliveryMethod = deliveryMethod,
				BillingAddress = billingAddress,
				ShippingAddress = shippingAddress,
				PaymentCard = paymentCard,
				EmailAddress = emailAddress,
				TelephoneNumber = telephoneNumber,
				MobileTelephoneNumber = mobileTelephoneNumber,
				Status = OrderStatus.Unpaid,
                TotalDeliveryPrice = deliveryPrice,
                TotalVatPrice = totalVatPrice,
                TotalPrice = totalPrice
			};
			foreach (OrderItem orderItem in items)
				orderItem.Parent = order;
			order.Parent = configuration.Orders;
			order.Save();

            order.Title = "Order #" + order.ID;
            order.Save();

            // Process payment.
            PaymentRequest paymentRequest = new PaymentRequest(
                PaymentTransactionType.Payment, order.ID.ToString(), order.TotalPrice, order.Title,
                order.BillingAddress, order.ShippingAddress,
                order.PaymentCard, cardNumber, cardVerificationCode, 
                order.TelephoneNumber, order.EmailAddress, _webContext.Request.UserHostAddress);
			
            PaymentResponse paymentResponse = _paymentGateway.TakePayment(paymentRequest);
            
            if (paymentResponse.Success)
			{
				// Update order status to Paid.
				order.Status = OrderStatus.Paid;
				order.Save();

				// Could send email to customer and vendor at this point.
			}
			else
			{
				throw new ZeusECommerceException(paymentResponse.Message);
			}

			return order;
		}

        public Order PlaceOrderWithoutPayment(IECommerceConfiguration configuration, DeliveryMethod deliveryMethod, 
            decimal deliveryPrice, Address billingAddress,
            Address shippingAddress, PaymentCard paymentCard, string emailAddress,
            string telephoneNumber, string mobileTelephoneNumber,
            IEnumerable<OrderItem> items,
            decimal totalVatPrice, decimal totalPrice)
        {
			try
			{

				// Convert shopping basket into order, with unpaid status.
				Order order = Order.Create(new Order
				{
					User = (_webContext.User != null && (_webContext.User is WebPrincipal)) ? ((WebPrincipal)_webContext.User).MembershipUser : null,
					DeliveryMethod = deliveryMethod,
					BillingAddress = billingAddress,
					ShippingAddress = shippingAddress,
					PaymentCard = paymentCard,
					EmailAddress = emailAddress,
					TelephoneNumber = telephoneNumber,
					MobileTelephoneNumber = mobileTelephoneNumber,
					Status = OrderStatus.Unpaid,
                    TotalDeliveryPrice = deliveryPrice,
                    TotalVatPrice = totalVatPrice,
                    TotalPrice = totalPrice,
					Parent = configuration.Orders
				});
				foreach (OrderItem orderItem in items)
				{
					orderItem.Parent = order;
					orderItem.Save();
				}

				if (_webContext.User != null && !string.IsNullOrEmpty(_webContext.User.Identity.Name))
                {
                    order.Title = _webContext.User.Identity.Name + " for " + items.First().Title;
                }
                else
                {
                    order.Title = "Order #" + order.ID;
                }

                order.Save();

				return order;
			}
			catch (System.Exception ex)
			{ 
				throw(new System.Exception("Error creating order: \n" + ex.Message + "\n" + ex.StackTrace));
			}			
        }

		public Order PlaceOrder(Shop shop, string cardNumber, string cardVerificationCode,
			ShoppingBasket shoppingBasket)
		{
			List<OrderItem> items = new List<OrderItem>();
			foreach (IShoppingBasketItem shoppingBasketItem in items)
			{
				ProductOrderItem orderItem = new ProductOrderItem
				{
					WeakProductLink = shoppingBasketItem.Product.ID,
					ProductTitle = shoppingBasketItem.Product.Title,
					Quantity = shoppingBasketItem.Quantity,
					Price = shoppingBasketItem.Product.CurrentPrice,
                    VATable = !(shoppingBasketItem.Product.VatZeroRated)
				};
				if (shoppingBasketItem.Variations != null)
					foreach (Variation variation in shoppingBasketItem.Variations)
						orderItem.Variations.Add(variation.VariationSet.Title + ": " + variation.Title);
				items.Add(orderItem);
			}

			Order order = PlaceOrder(shop, cardNumber, cardVerificationCode, shoppingBasket.DeliveryMethod,
				shoppingBasket.DeliveryMethod.Price, shoppingBasket.BillingAddress.Clone(),
				(shoppingBasket.ShippingAddress ?? shoppingBasket.BillingAddress).Clone(),
				shoppingBasket.PaymentCard.Clone(), shoppingBasket.EmailAddress,
				shoppingBasket.TelephoneNumber, shoppingBasket.MobileTelephoneNumber,
				items, shoppingBasket.TotalDeliveryPrice, shoppingBasket.TotalPrice);

			// Clear shopping basket.
			shoppingBasket.Destroy();

			return order;
		}
	}
}