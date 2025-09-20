
namespace DevNote
{
    public interface IStartHandler
    {
        public void Start();
    }

    public interface IUpdateHandler
    {
        public void Update();
    }

    public interface IFixedUpdateHandler
    {
        public void FixedUpdate();
    }

    public interface IDisposeHandler
    {
        public void Dispose();
    }



}


