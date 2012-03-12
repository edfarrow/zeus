using System;
using Zeus.ContentTypes;

namespace Zeus.Editors.Attributes
{
	public interface IEditorAttribute : IUniquelyNamed, IComparable<IEditorAttribute>
	{
		int SortOrder { get; set; }
		string Title { get; set; }
		IEditor GetEditor();
	}
}