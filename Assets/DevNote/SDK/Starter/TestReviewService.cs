using UnityEngine;

namespace DevNote.Services.Starter
{
    public class TestReviewService : MonoBehaviour, IReview
    {
        bool ISelectableService.Available => true;

        bool IProjectInitializable.Initialized => true;

        void IProjectInitializable.Initialize() { }

        void IReview.Request() => Debug.Log($"{Const.LOG_PREFIX} Review is requested");
    }
}

