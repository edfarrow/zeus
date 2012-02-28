using Ext.Net;
using MongoDB.Bson.Serialization.Attributes;
using Zeus.Editors.Attributes;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes.ReferenceData
{
	[ContentType("Currency")]
	[RestrictParents(typeof(CurrencyList))]
	public class Currency : BaseContentItem
	{
		public Currency()
		{

		}

		public Currency(string title, string isoCode, string symbol)
		{
			Title = title;
			IsoCode = isoCode;
			Symbol = symbol;

			Icon icon = Icon.MoneyAdd;
			switch (isoCode)
			{
				case "GBP" :
					icon = Icon.MoneyPound;
					break;
				case "USD":
					icon = Icon.MoneyDollar;
					break;
				case "EUR":
					icon = Icon.MoneyEuro;
					break;
				case "JPY":
					icon = Icon.MoneyYen;
					break;
			}
			CurrencyIcon = icon;
		}

		[TextBoxEditor("Name", 10, Required = true)]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}

		[TextBoxEditor("ISO Code", 100)]
		public string IsoCode { get; set; }

		[TextBoxEditor("Symbol", 110)]
		public string Symbol { get; set; }

		[TextBoxEditor("Exchange Rate", 120)]
		[BsonDefaultValue(1)]
		public decimal ExchangeRate { get; set; }

		[BsonDefaultValue(Icon.MoneyAdd)]
		public Icon CurrencyIcon { get; set; }

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(CurrencyIcon); }
		}
	}
}
