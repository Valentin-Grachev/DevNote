using System;

namespace DevNote
{

    public interface IHolder 
    {
        public bool RequireType(Type type);
    }


    public class Holder<T> : IHolder where T : class
    {
        public T Item { get; private set; }

        public bool Resolved => Item != null;

        public Holder()
        {
            Context.RegisterHolder(this);
        }

        public void Resolve(T value) => Item = value;

        bool IHolder.RequireType(Type type) => type == typeof(T);
    }
}

