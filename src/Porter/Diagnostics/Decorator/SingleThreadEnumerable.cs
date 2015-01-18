using System.Collections.Generic;
using System.Linq;

namespace Porter.Diagnostics.Decorator
{
	internal static class SingleThreadEnumerable
	{
		public static IEnumerable<T> Dispatch<T>(this IEnumerable<T> enumerable, ThreadDispatcher threadDispatcher)
		{
			return threadDispatcher.Process(() => enumerable.ToArray());
		}
	}
}