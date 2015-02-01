using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Porter.Models;

namespace Porter.Diagnostics.Decorator
{
	internal static class SingleThreadEnumerableExtensions
	{
		public static ISingleThreadEnumerable<T> Dispatch<T>(this IEnumerable<T> enumerable, ThreadDispatcher threadDispatcher)
		{
			return new SingleThreadEnumerable<T>(enumerable, threadDispatcher);
		}
	}


	internal class SingleThreadEnumerable<T> : ISingleThreadEnumerable<T>
	{
		private readonly IEnumerable<T> _enumerable;
		private readonly ThreadDispatcher _threadDispatcher;

		public SingleThreadEnumerable(IEnumerable<T> enumerable, ThreadDispatcher threadDispatcher)
		{
			_enumerable = enumerable;
			_threadDispatcher = threadDispatcher;
		}

		public IEnumerator<T> GetEnumerator()
		{
			if (_threadDispatcher.IsCurrentThread())
			{
				return _enumerable.GetEnumerator();
			}
			return ((IEnumerable<T>) ToArray()).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public T[] ToArray()
		{
			return _threadDispatcher.Process(() => _enumerable.ToArray());
		}
	}
}