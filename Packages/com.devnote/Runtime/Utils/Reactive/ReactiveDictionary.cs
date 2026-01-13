using System;
using System.Collections;
using System.Collections.Generic;

namespace DevNote
{
    public class ReactiveDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        public event Action OnChanged;


        private Dictionary<TKey, TValue> _dictionary;

        public ReactiveDictionary() => _dictionary = new Dictionary<TKey, TValue>();

        public ReactiveDictionary(int capacity) => _dictionary = new Dictionary<TKey, TValue>(capacity);

        public ReactiveDictionary(Dictionary<TKey, TValue> dictionary) => _dictionary = dictionary;



        public int Count => _dictionary.Count;

        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
            OnChanged?.Invoke();
        }

        public void Remove(TKey key)
        {
            if (!_dictionary.ContainsKey(key)) return;

            _dictionary.Remove(key);
            OnChanged?.Invoke();
        }

        public TValue Get(TKey key) => _dictionary[key];

        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

        public void Set(TKey key, TValue value)
        {
            _dictionary[key] = value;
            OnChanged?.Invoke();
        }

        public void Clear()
        {
            _dictionary.Clear();
            OnChanged?.Invoke();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() 
            => ((IEnumerable<KeyValuePair<TKey, TValue>>)_dictionary).GetEnumerator();


        IEnumerator IEnumerable.GetEnumerator() 
            => ((IEnumerable)_dictionary).GetEnumerator();

    }
}


