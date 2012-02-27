using System;
using System.Collections.Generic;

namespace Zeus.EditableTypes
{
	public interface IEditableTypeBuilder
	{
		IDictionary<Type, EditableType> GetEditableTypes();
	}
}