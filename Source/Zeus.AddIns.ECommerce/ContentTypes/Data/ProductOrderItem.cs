using System.Collections.Generic;
using MongoDB.Bson;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Product Order Item")]
	public class ProductOrderItem : OrderItem
	{
		public override string DisplayTitle
		{
			get { return ProductTitle; }
		}

		public ObjectId WeakProductLink { get; set; }

		public Pages.Product Product
		{
			get { return (Pages.Product) FindOneByID(WeakProductLink); }
		}

		public string ProductTitle { get; set; }

		public virtual List<string> Variations { get; set; }
	}
}