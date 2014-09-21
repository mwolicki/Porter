using System.Collections.Generic;

namespace Porter.Extensions
{
	public static class EnumerableExtensions
	{
		public static bool HasExactlyElements<T>(this IEnumerable<T> enumerable, int elements)
		{
			var enumerator = enumerable.GetEnumerator();
			for (int i = 0; i < elements; i++)
			{
				if (!enumerator.MoveNext())
				{
					return false;
				}
			}
			return !enumerator.MoveNext();
		}
	}
}