using UnityEngine;

namespace DevNote.SDK.Test
{
    public class TestReviewService : MonoBehaviour, IReview
    {
        bool ISelectableService.IsAvailableForSelection => true;

        bool IInitializable.Initialized => true;

        bool IReview.ReviewIsAvailable => true;

        void IInitializable.Initialize() { }

        void IReview.Rate() => Debug.Log($"{Info.Prefix} Review is requested");
    }
}

