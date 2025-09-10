using UnityEngine;

namespace DevNote.Services.Test
{
    public class TestReviewService : MonoBehaviour, IReview
    {
        bool ISelectableService.Available => true;

        bool IInitializable.Initialized => true;

        void IInitializable.Initialize() { }

        void IReview.Rate() => Debug.Log($"{Info.Prefix} Review is requested");
    }
}

