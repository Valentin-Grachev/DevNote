using System;
using UnityEngine;
using MirraSdk = MirraGames.SDK.MirraSDK;

namespace DevNote.SDK.MirraSDK
{
    public class MirraReviewService : MonoBehaviour, IReview
    {
        bool IInitializable.Initialized => MirraSdk.IsInitialized;

        bool ISelectableService.IsAvailableForSelection => MirraEnvironmentService.IsAvailableForSelection;

        bool IReview.ReviewIsAvailable => true;

        void IInitializable.Initialize() { }

        void IReview.Rate(Action onGameRated, Action onRejected)
        {
            MirraSdk.Platform.RateGame();
            IReview.GameRated(onGameRated);
        }
    }
}
