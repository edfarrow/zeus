using System;
using System.Linq;
using Zeus.BaseLibrary.ExtensionMethods.Linq;

namespace Zeus.Persistence
{
	public class Finder : IFinder
	{
		public IQueryable<T> QueryItems<T>() where T : ContentItem
		{
			return ContentItem.FindAll().OfType<T>();
		}

		public IQueryable<ContentItem> QueryItems()
		{
			return ContentItem.FindAll();
		}

		public IQueryable Query(Type resultType)
		{
			return ContentItem.FindAll().OfType(resultType);
		}
	}
}