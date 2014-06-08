using System;
using System.Collections.Generic;

namespace PorterApp.UnitTests.ChooseFileCommandTests
{
	public sealed class DelegateComparer<T> : IEqualityComparer<T>
	{
		private readonly Func<T, T, bool> _isEqual;

		public DelegateComparer(Func<T, T, bool> isEqual)
		{
			_isEqual = isEqual;
		}

		public bool Equals(T x, T y)
		{
			return _isEqual(x, y);
		}

		public int GetHashCode(T obj)
		{
			return 0;
		}
	}
}