using System;

namespace DevNote
{
    public interface IReview : IInitializable, ISelectableService
    {
        public static event Action OnGameRated; 

        public bool ReviewIsAvailable { get; }

        public void Rate(Action onGameRated = null, Action onRejected = null);


        protected static void GameRated(Action onGameRated)
        {
            onGameRated?.Invoke();
            OnGameRated?.Invoke();
        }
    }

}


