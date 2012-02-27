using System;
using Ormongo;
using Zeus.AddIns.ECommerce.PaymentGateways;
using Zeus.Editors.Attributes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	public class PaymentCard : EmbeddedDocument<ContentItem>
	{
		[TextBoxEditor("CardType", 200)]
		public PaymentCardType CardType { get; set; }

		[TextBoxEditor("Name On Card", 210)]
		public string NameOnCard { get; set; }

		[TextBoxEditor("MaskedCardNumber", 220)]
		public string MaskedCardNumber { get; set; }

		[TextBoxEditor("Expiry Month", 230, 10)]
		public int ExpiryMonth { get; set; }

		[TextBoxEditor("Expiry Year", 240, 10)]
		public int ExpiryYear { get; set; }

		public DateTime ValidTo
		{
			get
			{
				// Sets the date to the last day of the month.
				return new DateTime(ExpiryYear, ExpiryMonth, DateTime.DaysInMonth(ExpiryYear, ExpiryMonth));
			}
		}

		[TextBoxEditor("Start Month", 250)]
		public int? StartMonth { get; set; }

		[TextBoxEditor("Start Year", 260)]
		public int? StartYear { get; set; }

		public DateTime? ValidFrom
		{
			get
			{
				if (StartMonth != null && StartYear != null)
					// Sets the date to the first day of the month.
					return new DateTime(StartYear.Value, StartMonth.Value, 1);
				return null;
			}
		}

		[TextBoxEditor("Issue Number", 270)]
		public string IssueNumber { get; set; }

		public PaymentCard Clone()
		{
			return (PaymentCard) MemberwiseClone();
		}
	}
}