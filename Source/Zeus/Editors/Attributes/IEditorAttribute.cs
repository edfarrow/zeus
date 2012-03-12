using Zeus.ContentTypes;

namespace Zeus.Editors.Attributes
{
	public interface IEditorAttribute : IUniquelyNamed
	{
		IEditor GetEditor();
	}
}