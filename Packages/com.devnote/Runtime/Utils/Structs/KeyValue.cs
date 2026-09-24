using System;

namespace DevNote
{
    [Serializable] public struct KeyValue<T1, T2>
    {
        public T1 Key;
        public T2 Value;
    }
}
