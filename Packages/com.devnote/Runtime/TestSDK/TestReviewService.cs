using System;
using UnityEngine;

namespace DevNote.SDK.Test
{
    public class TestReviewService : MonoBehaviour, IReview
    {
        bool ISelectableService.IsAvailableForSelection => true;

        bool IInitializable.Initialized => true;

        bool IReview.ReviewIsAvailable => true;

        void IInitializable.Initialize() { }

        void IReview.Rate(Action onGameRated, Action onRejected)
        {
            Debug.Log($"{Info.Prefix} Game rated");
            IReview.GameRated(onGameRated);
        }
    }
}

