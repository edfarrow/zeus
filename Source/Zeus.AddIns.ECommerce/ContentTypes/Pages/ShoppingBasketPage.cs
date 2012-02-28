using Ext.Net;
using Zeus.Editors.Attributes;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType("Shopping Basket Page")]
	[RestrictParents(typeof(Shop))]
	public class ShoppingBasketPage : BasePage
	{
		protected override Icon Icon
		{
			get { return Icon.Basket; }
		}

		[HtmlTextBoxEditor("Extra Information", 210)]
		public string ExtraInformation { get; set; }
	}
}