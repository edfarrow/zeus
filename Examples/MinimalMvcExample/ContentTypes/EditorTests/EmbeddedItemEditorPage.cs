using Zeus.Editors.Attributes;
using Zeus.Templates.ContentTypes;
using Zeus.Web.UI;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes.EditorTests
{
	[ContentType]
	[Panel("BillingAddress", "Billing Address", 120)]
	public class EmbeddedItemEditorPage : BasePage
	{
		[TextBoxEditor("Person Name", 100)]
		public string PersonName { get; set; }

		[EmbeddedItemEditor("Address", 110)]
		public virtual Address ShippingAddress { get; set; }

		[EmbeddedItemEditor("Address", 0, ContainerName = "BillingAddress")]
		public virtual Address BillingAddress { get; set; }
	}

	public class Address : EmbeddedItem
	{
		[TextBoxEditor("Address 1", 10)]
		public virtual string Address1 { get; set; }

		[TextBoxEditor("Address 2", 10)]
		public virtual string Address2 { get; set; }
	}
}