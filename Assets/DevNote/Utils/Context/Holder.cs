using System;

namespace DevNote
{

    public interface IHolder 
    {
        public bool RequireType(Type type);
    }


    public class Holder<T> : IHolder where T : class
    {
        public T Value { get; private set; }

        public bool Resolved => Value != null;

        public Holder()
        {
            Context.RegisterHolder(this);
        }

        public void Resolve(T value) => Value = value;

        bool IHolder.RequireType(Type type) => type == typeof(T);
    }
}

