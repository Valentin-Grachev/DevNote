namespace DevNote
{
    public interface IInitializable
    {
        public bool Initialized { get; }

        public void Initialize();

    }

}
