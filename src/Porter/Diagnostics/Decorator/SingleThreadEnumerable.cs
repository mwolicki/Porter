using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Porter.Diagnostics.Decorator
{
	internal static class SingleThreadEnumerableExtensions
	{
		public static ISingleThreadEnumerable<T> Dispatch<T>(this IEnumerable<T> enumerable, ThreadDispatcher threadDispatcher)
		{
			return new SingleThreadEnumerable<T>(enumerable, threadDispatcher);
		}
	}


	public interface ISingleThreadEnumerable<T> : IEnumerable<T>
	{
		T[] ToArray();
		IEnumerable<T2> Select<T2>(Func<T, T2> s);
		IEnumerable<T> Take(int i);
		//List<T> ToList();
		//T Single();
		//T SingleOrDefault();
		//T First();
		//T FirstOrDefault();
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

		public IEnumerable<T2> Select<T2>(Func<T, T2> s)
		{
			return _enumerable.Select(s).Dispatch(_threadDispatcher);
		}

		public IEnumerable<T> Take(int i)
		{
			return _enumerable.Take(i).Dispatch(_threadDispatcher);
		}
	}
}