using System;
using System.Linq;

namespace Zeus.Persistence
{
	public interface IFinder
	{
		IQueryable<T> QueryItems<T>() where T : ContentItem;
		IQueryable<ContentItem> QueryItems();
		IQueryable Query(Type resultType);
	}
}