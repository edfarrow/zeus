using Ormongo;

namespace Zeus.Admin.Plugins.RecycleBin
{
	/// <summary>
	/// Container of scrapped items.
	/// </summary>
	public interface IRecycleBin
	{
		/// <summary>Whether the trash functionality is enabled.</summary>
		bool Enabled { get; }

		/// <summary>Currently thrown items.</summary>
		Relation<ContentItem> Children { get; }
	}
}