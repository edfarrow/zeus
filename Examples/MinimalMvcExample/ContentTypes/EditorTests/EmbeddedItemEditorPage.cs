using Zeus.Editors.Attributes;
using Zeus.Templates.ContentTypes;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes.EditorTests
{
	[ContentType]
	[Panel("BillingAddress", "Billing Address", 120)]
	public class EmbeddedItemEditorPage : BasePage
	{
		[TextBoxEditor("Product", 100)]
		public string Product { get; set; }

		[EmbeddedItemEditor("Address", 110)]
		public virtual Address ShippingAddress { get; set; }

		[EmbeddedItemEditor("Address", 0, ContainerName = "BillingAddress")]
		public virtual Address BillingAddress { get; set; }
	}
}