using System;
using System.Linq;
using System.Linq.Expressions;

namespace Zeus.BaseLibrary.ExtensionMethods.Linq
{
	public static class IQueryableExtensionMethods
	{
		public static IQueryable<TSource> OfType<TSource>(this IQueryable<TSource> source, Type type)
		{
			if (source == null)
				throw new ArgumentNullException("source");

			return source.Provider.CreateQuery(Expression.Call(null,
					(typeof(Queryable).GetMethod("OfType")).MakeGenericMethod(type), 
					source.Expression))
				.Cast<TSource>();
		}
	}
}