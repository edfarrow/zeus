using System.Collections.Generic;
using System.Linq;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.Design.Editors;
using Zeus.ContentProperties;
using Zeus.Design.Editors;
using Zeus.FileSystem.Images;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType(Name = "BaseProduct")]
	[RestrictParents(typeof(Category))]
	public class Product : BasePage
	{
		public override string IconUrl
		{
			get { return GetIconUrl(typeof(Product), "Zeus.AddIns.ECommerce.Icons.package.png"); }
		}

		[ContentProperty("Product Code", 200)]
		public string ProductCode
		{
			get { return GetDetail("ProductCode", string.Empty); }
			set { SetDetail("ProductCode", value); }
		}

		[ContentProperty("Regular Price", 210)]
		[TextBoxEditor(Required = true, EditorPrefixText = "�&nbsp;", TextBoxCssClass = "price")]
		public decimal RegularPrice
		{
			get { return GetDetail("RegularPrice", 0m); }
			set { SetDetail("RegularPrice", value); }
		}

		[ContentProperty("Sale Price", 220)]
		[TextBoxEditor(EditorPrefixText = "�&nbsp;", TextBoxCssClass = "price")]
		public decimal? SalePrice
		{
			get { return GetDetail<decimal?>("SalePrice", null); }
			set { SetDetail("SalePrice", value); }
		}

		public decimal CurrentPrice
		{
			get { return SalePrice ?? RegularPrice; }
		}

		[ChildEditor("Main Image", 230)]
		public Image MainImage
		{
			get { return GetChild("MainImage") as Image; }
			set
			{
				if (value != null)
				{
					value.Name = "MainImage";
					value.AddTo(this);
				}
			}
		}

		[MultiImageUploadEditor("Extra Images", 250)]
		public PropertyCollection ExtraImages
		{
			get { return GetDetailCollection("ExtraImages", true); }
		}

		[VariationConfigurationEditor("Variations", 260)]
		public IEnumerable<VariationConfiguration> VariationConfigurations
		{
			get { return GetChildren<VariationConfiguration>(); }
		}

		public IEnumerable<VariationSet> AvailableVariationSets
		{
			get
			{
				return VariationConfigurations
					.SelectMany(vc => vc.Permutation.Variations.Cast<Variation>().Select(v => v.VariationSet))
					.Distinct();
			}
		}

		public Category CurrentCategory
		{
			get { return (Category) ((Parent is Category) ? Parent : Parent.Parent); }
		}
	}
}