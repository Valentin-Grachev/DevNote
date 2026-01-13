using System;

namespace DevNote
{
    public class ReactiveValue<T>
    {
        public event Action OnChanged;

        private T _value;

        public T Value 
        { 
            get => _value;
            set
            {
                _value = value;
                OnChanged?.Invoke();
            }
        }

        public ReactiveValue(T value)
        {
            _value = value;
        }

        public override string ToString() => _value.ToString();

    }
}

