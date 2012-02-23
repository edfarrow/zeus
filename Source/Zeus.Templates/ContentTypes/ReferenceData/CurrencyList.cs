using Ext.Net;
using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.Templates.ContentTypes.ReferenceData
{
	[ContentType("Currency List", Description = "Container for currencies")]
	[RestrictParents(typeof(ReferenceDataNode))]
	public class CurrencyList : BaseContentItem
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

		[LinkedItemDropDownListEditor("Base Currency", 100)]
		public virtual Currency BaseCurrency { get; set; }

		protected override void OnAfterCreate()
		{
			Children.Create(new Currency("US Dollar", "USD", "$"));
			Children.Create(new Currency("British Pound Sterling", "GBP", "£"));
			Children.Create(new Currency("Euro", "EUR", "€"));
			base.OnAfterCreate();
		}
	}
}