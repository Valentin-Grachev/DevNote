using System;
using UnityEngine;



namespace DevNote.SDK.YandexGames
{
    public class YandexGamesReviewService : MonoBehaviour, IReview
    {
        bool IInitializable.Initialized => true;

        bool ISelectableService.IsAvailableForSelection => YG_Sdk.IsAvailableForSelection;

        bool IReview.ReviewIsAvailable => true;

        void IInitializable.Initialize() { }

        void IReview.Rate(Action onGameRated, Action onRejected)
        {
            YG_Review.Request();
            IReview.GameRated(onGameRated);
        }
    }
}


