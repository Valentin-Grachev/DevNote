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


        void IReview.Rate()
        {
            GP_App.ReviewRequest();
        }
    }
}

