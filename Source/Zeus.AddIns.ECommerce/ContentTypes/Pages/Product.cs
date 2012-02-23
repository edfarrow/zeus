using System.Collections.Generic;
using System.Linq;
using Ext.Net;
using Ormongo;
using Zeus.AddIns.ECommerce.ContentTypes.Data;
using Zeus.AddIns.ECommerce.Design.Editors;
using Zeus.Design.Editors;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;
using Zeus.Web.UI;
using Image = Zeus.FileSystem.Images.Image;

namespace Zeus.AddIns.ECommerce.ContentTypes.Pages
{
	[ContentType(Name = "BaseProduct")]
	[RestrictParents(typeof(Category))]
	[TabPanel("Images", "Images", 200)]
	public class Product : BasePage
	{
		protected override Icon Icon
		{
			get { return Icon.Package; }
		}

		[TextBoxEditor("Product Code", 200)]
		public virtual string ProductCode { get; set; }

		[TextBoxEditor("Regular Price", 210, Required = true, EditorPrefixText = "£&nbsp;", TextBoxCssClass = "price")]
		public decimal RegularPrice { get; set; }

		[TextBoxEditor("Sale Price", 220, EditorPrefixText = "£&nbsp;", TextBoxCssClass = "price")]
		public virtual decimal? SalePrice { get; set; }

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
					value.Parent = this;
				}
			}
		}

		[MultiImageUploadEditor("Extra Images", 250, ContainerName = "Images")]
		public List<Attachment> ExtraImages { get; set; }

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
					.SelectMany(vc => vc.Permutation.Variations.Select(v => v.VariationSet))
					.Distinct();
			}
		}

		public Category CurrentCategory
		{
			get { return (Category) ((Parent is Category) ? Parent : Parent.Parent); }
		}

		[CheckBoxEditor("Item is Out of Stock", "", 300)]
		public virtual bool OutOfStock { get; set; }

		[CheckBoxEditor("VAT Zero Rated", "", 300, Description = "Please check if VAT is not added to this product")]
		public virtual bool VatZeroRated { get; set; }
	}
}