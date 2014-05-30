using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Porter
{
	public sealed class MultiElementDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
	{
		private readonly Dictionary<TKey, LinkedList<TValue>> _elements = new Dictionary<TKey, LinkedList<TValue>>();

		public TValue this[TKey key]
		{
			get { return _elements[key].First.Value; }
			set
			{
				Add(key, value);
			}
		}

		public void Add(TKey key, TValue value)
		{
			if (!_elements.ContainsKey(key))
			{
				_elements[key] = new LinkedList<TValue>();
			}

			_elements[key].AddLast(value);
		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			return (
				from outerElement in _elements
				from innerElement in outerElement.Value
				select new KeyValuePair<TKey, TValue>(outerElement.Key, innerElement)
				).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}