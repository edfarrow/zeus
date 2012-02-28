using System.Collections.Generic;
using Zeus.Editors.Attributes;
using Zeus.FileSystem;
using Zeus.Templates.ContentTypes;

namespace Zeus.Examples.MinimalMvcExample.ContentTypes.EditorTests
{
	[ContentType]
	public class EmbeddedFileCollectionEditorPage : BasePage
	{
		[EmbeddedFileCollectionEditor("Files", 100)]
		public virtual List<EmbeddedFile> Files { get; set; }
	}
}