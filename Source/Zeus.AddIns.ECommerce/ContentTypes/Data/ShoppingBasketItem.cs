using System.Collections.Generic;
using System.Linq;
using Zeus.AddIns.ECommerce.ContentTypes.Pages;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.AddIns.ECommerce.Services;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Shopping Basket Item")]
	[RestrictParents(typeof(ShoppingBasket))]
	public class ShoppingBasketItem : BaseContentItem, IShoppingBasketItem
	{
		public Product Product { get; set; }
		public int Quantity { get; set; }

		public decimal LineTotal
		{
			get { return Product.CurrentPrice * Quantity; }
		}

		public VariationPermutation VariationPermutation
		{
			get { return GetChild("variation-permutation") as VariationPermutation; }
			set
			{
				if (value != null)
				{
					value.Name = "variation-permutation";
					value.AddTo(this);
				}
			}
		}

		public IEnumerable<Variation> Variations
		{
			get
			{
				if (VariationPermutation != null)
					return VariationPermutation.Variations.Cast<Variation>();
				return null;
			}
		}
	}
}