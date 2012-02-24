using Ext.Net;
using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("ECommerce AddIn")]
	[RestrictParents(typeof(AddInContainer))]
	public class ECommerceAddIn : DataContentItem, IECommerceConfiguration
	{
		private const string OrdersName = "orders";

		public ECommerceAddIn()
		{
			Title = "ECommerce";
			Name = "ecommerce";
		}

		protected override Icon Icon
		{
			get { return Icon.Plugin; }
		}

		[TextBoxEditor("Confirmation Email From", 220)]
		public string ConfirmationEmailFrom { get; set; }

		[TextBoxEditor("Confirmation Email Text", 221, TextMode = System.Web.UI.WebControls.TextBoxMode.MultiLine)]
		public string ConfirmationEmailText { get; set; }

		[TextBoxEditor("Vendor Email", 222, Description = "This is the email address which will receive the vendor's copy of the order confirmation email.")]
		public string VendorEmail { get; set; }

		public OrderContainer Orders
		{
			get { return GetChild(OrdersName) as OrderContainer; }
		}

		protected override void OnAfterCreate()
		{
			Children.Create(new OrderContainer
			{
				Name = OrdersName,
				Title = "Orders"
			});

			base.OnAfterCreate();
		}
	}
}