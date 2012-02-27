using System.Collections.Generic;

namespace Zeus.EditableTypes
{
	/// <summary>
	/// Attributes implementing this interface can alter editable types while
	/// they are being initiated. All classes in the inheritance chain are 
	/// queried for this interface when refining the definition.
	/// </summary>
	public interface IInheritableEditableTypeRefiner
	{
		/// <summary>Alters the item definition.</summary>
		/// <param name="currentEditableType">The editable type to alter.</param>
		/// <param name="allEditableTypes">All editable type.</param>
		void Refine(EditableType currentEditableType, IList<EditableType> allEditableTypes);
	}
}