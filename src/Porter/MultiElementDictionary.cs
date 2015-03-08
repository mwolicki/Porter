using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Porter
{
	internal sealed class MultiElementDictionary<TKey, TValue> : IMultiElementDictionary<TKey, TValue>
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
			LinkedList<TValue> linkedList;
			if (!_elements.TryGetValue(key, out linkedList))
			{
				linkedList = new LinkedList<TValue>();
				_elements[key] = linkedList;
			}

			linkedList.AddLast(value);
		}

		//TODO: Change this to return IEnumerable - check if we need current implementation
		[Obsolete("Change this to return IEnumerable - check if we need current implementation")]
		public Dictionary<TKey, LinkedList<TValue>> GetDictionary()
		{
			return _elements;
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
	
	internal sealed class MultiElementDictionary2<TValue> : IMultiElementDictionary<string, TValue>
	{
		private readonly Dictionary<string, List<TValue>> _elements = new Dictionary<string, List<TValue>>();
		private LastElements _history = new LastElements(new object());


		public TValue this[string key]
		{
			get { return _elements[key].First(); }
			set
			{
				Add(key, value);
			}
		}

		struct LastElements
		{
			private const int Size = 16;
			private readonly string[] _historyKey;
			private readonly int[] _historyKeyHashCode;
			private readonly List<TValue>[] _historyValue;
			private int _index;

			public LastElements(object o)
			{
				_historyKey = new string[Size];
				_historyKeyHashCode = new int[Size];
				_historyValue = new List<TValue>[Size];
				_index = 0;
			}

			public unsafe void Set(string key, List<TValue> value)
			{
				_historyKey[_index] = key;
				_historyKeyHashCode[_index] = key.GetHashCode();
				_historyValue[_index] = value;
				_index = (_index + 1)%Size;
			}
			
			public unsafe bool TryGet(string key, out List<TValue> value)
			{
				int hashCode = key.GetHashCode();
				fixed (int* hashArr = _historyKeyHashCode)
				for (int index = 0; index < Size; index++)
				{
					if (hashCode == hashArr[index] && key.Equals(_historyKey[index]))
					{
						value = _historyValue[index];
						return true;
					}
				}
				value = default(List<TValue>);
				return false;
			}
		}

		public void Add(string key, TValue value)
		{

			List<TValue> list;
			if (!_history.TryGet(key, out list))
			{
				if (!_elements.TryGetValue(key, out list))
				{
					list = new List<TValue>();
					_elements[key] = list;
				}
				_history.Set(key, list);
			}

			list.Add(value);
		}

		//TODO: Change this to return IEnumerable - check if we need current implementation
		[Obsolete("Change this to return IEnumerable - check if we need current implementation")]
		public Dictionary<string, List<TValue>> GetDictionary()
		{
			return _elements;
		}

		public IEnumerator<KeyValuePair<string, TValue>> GetEnumerator()
		{
			return (
				from outerElement in _elements
				from innerElement in outerElement.Value
				select new KeyValuePair<string, TValue>(outerElement.Key, innerElement)
				).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}