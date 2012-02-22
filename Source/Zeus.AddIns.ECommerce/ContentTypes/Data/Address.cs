using Ext.Net;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Templates.ContentTypes.ReferenceData;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType]
	[RestrictParents(typeof(ShoppingBasket))]
	public class Address : BaseContentItem
	{
		protected override Icon Icon
		{
			get { return Icon.EmailEdit; }
		}

		[TextBoxEditor("Title", 200)]
		public string PersonTitle { get; set; }

		[TextBoxEditor("First Name", 210)]
		public string FirstName { get; set; }

		[TextBoxEditor("Surname", 220)]
		public string Surname { get; set; }

		[TextBoxEditor("Address Line One", 230)]
		public string AddressLine1 { get; set; }

		[TextBoxEditor("Address Line 2", 240)]
		public string AddressLine2 { get; set; }

		[TextBoxEditor("Town / City", 250)]
		public string TownCity { get; set; }

		[TextBoxEditor("State / Region", 260)]
		public string StateRegion { get; set; }

		[TextBoxEditor("Postcode", 270)]
		public string Postcode { get; set; }

		[TextBoxEditor("Country", 280)]
		public Country Country { get; set; }

		public string PrintAddress
		{
			get
			{
				return AddressLine1 + "<br/>" +
					AddressLine2 + "<br/>" +
					TownCity + "<br/>" +
					StateRegion + "<br/>" +
					Postcode + "<br/>" +
					Country.Title;
			}
		}
	}
}