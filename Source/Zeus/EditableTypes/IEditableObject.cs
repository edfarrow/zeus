using System.Collections.Generic;

namespace Zeus.EditableTypes
{
	public interface IEditableObject
	{
		Dictionary<string, object> ExtraData { get; set; }
		object this[string key] { get; set; }
	}
}