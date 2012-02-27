using System.Collections.Generic;

namespace Zeus.EditableTypes
{
	public interface IEditableHierarchyBuilder<T>
		where T : IContainable
	{
		IEditorContainer Build(IEnumerable<IEditorContainer> containers, IEnumerable<T> editables);
	}
}