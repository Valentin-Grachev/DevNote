using UnityEngine;

namespace DevNote.Services.Test
{
    public class TestReviewService : MonoBehaviour, IReview
    {
        bool ISelectableService.Available => true;

        bool IProjectInitializable.Initialized => true;

        void IProjectInitializable.Initialize() { }

        void IReview.Request() => Debug.Log($"{Info.Prefix} Review is requested");
    }
}

