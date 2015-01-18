using System.Collections.Generic;

namespace Porter.Diagnostics.Decorator
{
	internal static class SingleThreadEnumerable
	{
		public static IEnumerable<T> Dispatch<T>(this IEnumerable<T> enumerable, ThreadDispatcher threadDispatcher)
		{
			var enumerator = threadDispatcher.Process(() => enumerable.GetEnumerator());
			while (threadDispatcher.Process(() => enumerator.MoveNext()))
			{
				yield return enumerator.Current;
			}
		}
	}
}