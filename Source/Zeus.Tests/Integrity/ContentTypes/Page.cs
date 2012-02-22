using Zeus.ContentTypes;
using Zeus.Design.Editors;
using Zeus.Integrity;

namespace Zeus.Tests.Integrity.ContentTypes
{
	[ContentType("Page", "ContentTypesPage")]
	[RestrictParents(typeof(StartPage))]
	public class Page : ContentItem
	{
		[TextBoxEditor("My Property", 100)]
		[PropertyAuthorizedRoles("ACertainGroup")]
		public virtual string MyProperty { get; set; }
	}
}