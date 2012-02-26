using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace Zeus.BaseLibrary.ExtensionMethods.Linq
{
	public static class IEnumerableExtensionMethods
	{
		#region Distinct

		public static IEnumerable<TSource> Distinct<TSource, TResult>(
			this IEnumerable<TSource> source, Func<TSource, TResult> comparer)
		{
			return source.Distinct(new DynamicComparer<TSource, TResult>(comparer));
		}

		private class DynamicComparer<T, TResult> : IEqualityComparer<T>
		{
			private readonly Func<T, TResult> _selector;

			public DynamicComparer(Func<T, TResult> selector)
			{
				_selector = selector;
			}

			public bool Equals(T x, T y)
			{
				TResult result1 = _selector(x);
				TResult result2 = _selector(y);
				return result1.Equals(result2);
			}

			public int GetHashCode(T obj)
			{
				TResult result = _selector(obj);
				return result.GetHashCode();
			}
		}

		#endregion

		public static string Join<T>(this IEnumerable<T> source, Func<T, string> valueCallback, string separator)
		{
			T[] values = source.ToArray();
			StringBuilder sb = new StringBuilder();
			for (int i = 0, length = values.Length; i < length; i++)
			{
				sb.Append(valueCallback(values[i]));
				if (i < length - 1)
					sb.Append(separator);
			}
			return sb.ToString();
		}
	}
}