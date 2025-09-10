using GamePush;
using UnityEngine;


namespace DevNote.Services.GamePush
{
    public class GamePushReviewService : MonoBehaviour, IReview
    {
        bool ISelectableService.Available => GamePushEnvironmentService.ServicesIsAvailable;

        bool IInitializable.Initialized => GP_Init.isReady;

        void IInitializable.Initialize() { }

        void IReview.Rate() => GP_App.ReviewRequest();

    }
}




