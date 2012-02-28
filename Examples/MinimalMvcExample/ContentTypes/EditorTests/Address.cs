using Zeus.Editors.Attributes;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes.EditorTests
{
	public class Address : EmbeddedItem
	{
		[EmbeddedItemEditor("Customer", 0)]
		public virtual PersonName Customer { get; set; }

		[TextBoxEditor("Address 1", 10)]
		public virtual string Address1 { get; set; }

		[TextBoxEditor("Address 2", 10)]
		public virtual string Address2 { get; set; }
	}

	public class PersonName : EmbeddedItem
	{
		[TextBoxEditor("Title", 10)]
		public string Title { get; set; }

		[TextBoxEditor("First Name", 20)]
		public string FirstName { get; set; }

		[TextBoxEditor("Last Name", 30)]
		public string LastName { get; set; }
	}
}