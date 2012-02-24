using Ormongo;

namespace Zeus.FileSystem
{
	public class EmbeddedFile : EmbeddedDocument<ContentItem>
	{
		public Attachment Data { get; set; }
		public string Caption { get; set; }
	}
}