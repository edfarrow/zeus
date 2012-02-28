using System;
using System.Collections.Generic;
using Ormongo;
using Zeus.ContentTypes;

namespace Zeus.EditableTypes
{
	public class EditableTypeManager : IEditableTypeManager
	{
		#region Fields

		private readonly IDictionary<Type, EditableType> _editableTypes;

		#endregion

		#region Constructor

		public EditableTypeManager(IEditableTypeBuilder editableTypeBuilder)
		{
			_editableTypes = editableTypeBuilder.GetEditableTypes();
		}

		#endregion

		#region Methods

		public ICollection<EditableType> GetEditableTypes()
		{
			return _editableTypes.Values;
		}

		public EditableType GetEditableType(object value)
		{
			Type underlyingType = (value is IProxy) ? ((IProxy) value).GetUnderlyingType() : value.GetType();
			return GetEditableType(underlyingType);
		}

		public EditableType GetEditableType(Type type)
		{
			if (_editableTypes.ContainsKey(type))
				return _editableTypes[type];
			return null;
		}

		#endregion
	}
}