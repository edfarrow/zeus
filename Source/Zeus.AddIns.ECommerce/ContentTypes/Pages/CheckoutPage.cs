using Ext.Net;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType("Checkout Page")]
	[RestrictParents(typeof(Shop))]
	public class CheckoutPage : BasePage
	{
		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.MoneyPound); }
		}

		[HtmlTextBoxEditor("Extra Information", 210)]
		public string ExtraInformation { get; set; }
	}
}