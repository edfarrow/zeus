namespace Zeus.EditableTypes
{
	public interface IEditableObject
	{
		object this[string key] { get; set; }
	}
}