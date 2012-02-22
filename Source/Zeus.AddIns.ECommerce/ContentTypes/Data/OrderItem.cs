using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[RestrictParents(typeof(Order))]
	public abstract class OrderItem : BaseContentItem
	{
		public abstract string DisplayTitle { get; }

		public int Quantity { get; set; }
		public decimal Price { get; set; }
		public bool VATable { get; set; }

        public decimal LineTotal
		{
			get { return Price * Quantity; }
		}
	}
}