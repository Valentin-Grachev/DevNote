
namespace DevNote
{
    public interface IStartable
    {
        public void Start();
    }

    public interface IUpdatable
    {
        public void Update();
    }

    public interface IFixedUpdatable
    {
        public void FixedUpdate();
    }

    public interface IContextDisposable
    {
        public void Dispose();
    }



}


