using System.Collections;
using System.Collections.Generic;

namespace Cachetwo
{
    /// <summary>
    /// A dictionary that implements <see cref="IReadOnlyDictionary{TKey, TValue}"/>
    /// but not <see cref="IDictionary"/> or <see cref="IDictionary{TKey, TValue}"/>.
    /// </summary>
    public class CustomReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> innerDictionary;

        public CustomReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
        {
            innerDictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        public TValue this[TKey key] => innerDictionary[key];

        public IEnumerable<TKey> Keys => ((IReadOnlyDictionary<TKey, TValue>)innerDictionary).Keys;

        public IEnumerable<TValue> Values => ((IReadOnlyDictionary<TKey, TValue>)innerDictionary).Values;

        public int Count => innerDictionary.Count;

        public bool ContainsKey(TKey key)
        {
            return innerDictionary.ContainsKey(key);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((IReadOnlyDictionary<TKey, TValue>)innerDictionary).GetEnumerator();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return innerDictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IReadOnlyDictionary<TKey, TValue>)innerDictionary).GetEnumerator();
        }
    }
}