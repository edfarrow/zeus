using System.Collections.Generic;
using Zeus.Editors.Attributes;
using Zeus.Templates.ContentTypes;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes.EditorTests
{
	[ContentType]
	public class StringCollectionEditorPage : BasePage
	{
		[StringCollectionEditor("Book Titles", 200)]
		public virtual List<string> BookTitles { get; set; }
	}
}