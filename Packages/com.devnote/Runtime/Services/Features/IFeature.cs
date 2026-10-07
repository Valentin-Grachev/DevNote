namespace DevNote
{
    public interface IFeature : IInitializable, ISelectableService
    {
        public bool IsStartMenu { get; }

    }
}
