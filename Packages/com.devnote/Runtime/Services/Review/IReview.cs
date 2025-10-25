namespace DevNote
{
    public interface IReview : IInitializable, ISelectableService
    {
        public bool ReviewIsAvailable { get; }

        public void Rate();
    }

}


