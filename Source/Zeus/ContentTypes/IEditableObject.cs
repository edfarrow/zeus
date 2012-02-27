namespace Zeus.ContentTypes
{
	public interface IEditableObject
	{
		object this[string key] { get; set; }
	}
}