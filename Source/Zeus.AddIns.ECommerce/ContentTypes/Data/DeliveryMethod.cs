using Ext.Net;
using Zeus.Editors.Attributes;
using Zeus.Integrity;
using Zeus.Templates.ContentTypes;

namespace Zeus.AddIns.ECommerce.ContentTypes.Data
{
	[ContentType("Delivery Method")]
	[RestrictParents(typeof(DeliveryMethodContainer))]
	public class DeliveryMethod : BaseContentItem
	{
		protected override Icon Icon
		{
			get { return Icon.Lorry; }
		}

		[TextBoxEditor("Price", 200, Required = true, EditorPrefixText = "£&nbsp;", TextBoxCssClass = "price")]
		public virtual decimal Price { get; set; }
	}
}