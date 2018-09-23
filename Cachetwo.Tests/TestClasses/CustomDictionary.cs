using System.Collections;
using System.Collections.Generic;

namespace Cachetwo
{
    /// <summary>
    /// A dictionary that implements <see cref="IDictionary{TKey, TValue}"/>
    /// but not <see cref="IDictionary"/>.
    /// </summary>
    public class CustomDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> innerDictionary = new Dictionary<TKey, TValue>();

        public TValue this[TKey key] { get => innerDictionary[key]; set => innerDictionary[key] = value; }

        public ICollection<TKey> Keys => ((IDictionary<TKey, TValue>)innerDictionary).Keys;

        public ICollection<TValue> Values => ((IDictionary<TKey, TValue>)innerDictionary).Values;

        public int Count => innerDictionary.Count;

        public bool IsReadOnly => ((IDictionary<TKey, TValue>)innerDictionary).IsReadOnly;

        public void Add(TKey key, TValue value)
        {
            innerDictionary.Add(key, value);
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ((IDictionary<TKey, TValue>)innerDictionary).Add(item);
        }

        public void Clear()
        {
            innerDictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)innerDictionary).Contains(item);
        }

        public bool ContainsKey(TKey key)
        {
            return innerDictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((IDictionary<TKey, TValue>)innerDictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((IDictionary<TKey, TValue>)innerDictionary).GetEnumerator();
        }

        public bool Remove(TKey key)
        {
            return innerDictionary.Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)innerDictionary).Remove(item);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return innerDictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<TKey, TValue>)innerDictionary).GetEnumerator();
        }
    }
}