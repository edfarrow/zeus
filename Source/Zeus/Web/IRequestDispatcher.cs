namespace Zeus.Web
{
	/// <summary>
	/// Resolves and constructs the controller used to further control 
	/// various aspects related to content items.
	/// </summary>
	public interface IRequestDispatcher
	{
		PathData ResolvePath();
	}
}