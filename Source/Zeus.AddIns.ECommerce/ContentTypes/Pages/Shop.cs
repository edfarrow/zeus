using System.Collections.Generic;
using System.Linq;
using Ext.Net;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.Editors.Attributes;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Web;
using Zeus.Web.UI;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType(Name = "BaseShop")]
	[TabPanel("ECommerce", "E-Commerce", 100)]
	[RestrictParents(typeof(WebsiteNode), typeof(Page))]
	public class Shop : BasePage, IECommerceConfiguration
	{
		private const string VariationContainerName = "variations";
		private const string ShoppingBasketsName = "shopping-baskets";
		private const string OrdersName = "orders";
		private const string DeliveryMethodsName = "delivery-methods";
		private const string CheckoutName = "checkout";

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.Money); }
		}

		public VariationSetContainer VariationsSet
		{
			get { return GetChild(VariationContainerName) as VariationSetContainer; }
		}

		public ShoppingBasketContainer ShoppingBaskets
		{
			get { return GetChild(ShoppingBasketsName) as ShoppingBasketContainer; }
		}

		public OrderContainer Orders
		{
			get { return GetChild(OrdersName) as OrderContainer; }
		}

		public DeliveryMethodContainer DeliveryMethods
		{
			get { return GetChild(DeliveryMethodsName) as DeliveryMethodContainer; }
		}

		public CheckoutPage CheckoutPage
		{
			get { return GetChild(CheckoutName) as CheckoutPage; }
		}

		public ShoppingBasketPage ShoppingBasketPage
		{
			get { return GetChildren<ShoppingBasketPage>().FirstOrDefault(); }
		}

		[TextBoxEditor("VAT, %", 199, 10, ContainerName = "ECommerce")]
		public decimal VAT { get; set; }

		[StringCollectionEditor("Titles", 200, ContainerName = "ECommerce")]
		public virtual List<string> Titles { get; set; }

		[LinkedItemDropDownListEditor("Contact Page", 210, ContainerName = "ECommerce")]
		public ContentItem ContactPage { get; set; }

		[TextBoxEditor("Confirmation Email From", 220, ContainerName = "ECommerce")]
		public string ConfirmationEmailFrom { get; set; }

		[TextBoxEditor("Confirmation Email Text", 221, ContainerName = "ECommerce", TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine)]
		public string ConfirmationEmailText { get; set; }

		[TextBoxEditor("Vendor Email", 222, ContainerName = "ECommerce", Description = "This is the email address which will receive the vendor's copy of the order confirmation email.")]
		public string VendorEmail { get; set; }

		[CheckBoxEditor("Persistent Shopping Baskets", "", 222, ContainerName = "ECommerce", Description = "Check this box if you want shopping baskets to persist, even after the customer closes their browser.")]
		public bool PersistentShoppingBaskets { get; set; }

		protected override void OnAfterCreate()
		{
			Create(new VariationSetContainer
			{
				Name = VariationContainerName,
				Title = "VariationsSet",
				Parent = this
			});

			Create(new ShoppingBasketContainer
			{
				Name = ShoppingBasketsName,
				Title = "Shopping Baskets",
				Parent = this
			});

			Create(new OrderContainer
			{
				Name = OrdersName,
				Title = "Orders",
				Parent = this
			});

			Create(new DeliveryMethodContainer
			{
				Name = DeliveryMethodsName,
				Title = "Delivery Methods",
				Parent = this
			});

			Create(new CheckoutPage
			{
				Name = CheckoutName,
				Title = "Checkout",
				Parent = this
			});

			base.OnAfterCreate();
		}
	}
}