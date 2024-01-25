using System;
using System.Collections.Generic;
using System.Linq;

namespace DbDiagramIo.Core
{
	public static class EnumerableExtensions
	{
		public static bool AllSafe<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate, bool valueIfNull = false)
		{
			return source?.All(predicate) ?? valueIfNull;
		}
	}
}