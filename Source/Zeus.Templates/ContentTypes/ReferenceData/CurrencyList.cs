using Ext.Net;
using Zeus.ContentTypes;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes.ReferenceData
{
	[ContentType("Currency List", Description = "Container for currencies")]
	[RestrictParents(typeof(ReferenceDataNode))]
	public class CurrencyList : BaseContentItem, ISelfPopulator
	{
		public CurrencyList()
		{
			Name = "currencies";
			Title = "Currencies";
		}

		public override string IconUrl
		{
			get { return Utility.GetCooliteIconUrl(Icon.Money); }
		}

		[ContentProperty("Base Currency", 100)]
		public Currency BaseCurrency
		{
			get { return GetDetail<Currency>("BaseCurrency", null); }
			set { SetDetail("BaseCurrency", value); }
		}

		#region ISelfPopulator Members

		public void Populate()
		{
			Children.Create(new Currency("US Dollar", "USD", "$"));
			Children.Create(new Currency("British Pound Sterling", "GBP", "£"));
			Children.Create(new Currency("Euro", "EUR", "€"));
		}

		#endregion
	}
}