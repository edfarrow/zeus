using Ext.Net;
using Zeus.Editors.Attributes;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType("Checkout Page")]
	[RestrictParents(typeof(Shop))]
	public class CheckoutPage : BasePage
	{
		protected override Icon Icon
		{
			get { return Icon.MoneyPound; }
		}

		[HtmlTextBoxEditor("Extra Information", 210)]
		public string ExtraInformation { get; set; }
	}
}