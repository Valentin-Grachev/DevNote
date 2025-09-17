using System;
using System.Collections;
using System.Collections.Generic;

namespace DevNote
{
    public class ReactiveList<T> : IEnumerable<T>
    {
        public event Action OnChanged;
        private List<T> _list;


        public ReactiveList() => _list = new List<T>();

        public ReactiveList(int capacity) => _list = new List<T>(capacity);

        public ReactiveList(List<T> list) => _list = list;



        public int Count => _list.Count;

        public T Find(Predicate<T> predicate) => _list.Find(predicate);

        public bool Exists(Predicate<T> predicate) => _list.Exists(predicate);


        public void Add(T item)
        {
            _list.Add(item);
            OnChanged?.Invoke();
        }

        public void Remove(T item)
        {
            if (!_list.Contains(item)) return;

            _list.Remove(item);
            OnChanged?.Invoke();
        }

        public T Get(int index) => _list[index];

        public void Set(int index, T value)
        {
            _list[index] = value;
            OnChanged?.Invoke();
        }

        public void Clear()
        {
            _list.Clear();
            OnChanged?.Invoke();
        }

        public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)_list).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_list).GetEnumerator();


        public void ReplaceList(List<T> list)
        {
            _list = list;
            OnChanged?.Invoke();
        }

    }
}



