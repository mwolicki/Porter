using System.Collections.Generic;

namespace Porter
{
	public interface IMultiElementDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
	{
		TValue this[TKey key] { get; set; }
	}
}