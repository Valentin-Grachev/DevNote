using System;
using GamePush;
using UnityEngine;

namespace DevNote.SDK.GamePush
{
    public class GamePushReviewService : MonoBehaviour, IReview
    {
        bool IInitializable.Initialized => GP_Init.isReady;

        bool ISelectableService.IsAvailableForSelection => GamePushEnvironmentService.IsAvailableForSelection;

        bool IReview.ReviewIsAvailable => GP_App.CanReview() && !GP_App.IsAlreadyReviewed();

        void IInitializable.Initialize() { }

        void IReview.Rate(Action onGameRated, Action onRejected)
        {
            bool gameRated = false;

            GP_App.ReviewRequest(
            onReviewResult: (result) =>
            {
                IReview.GameRated(onGameRated);
                gameRated = true;
            },
            onReviewClose: (error) =>
            {
                if (!gameRated) onRejected?.Invoke();
            });

        }
    }
}

