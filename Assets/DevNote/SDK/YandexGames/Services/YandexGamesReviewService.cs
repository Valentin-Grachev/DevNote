using DevNote.YandexGamesSDK;
using UnityEngine;



namespace DevNote.Services.YandexGames
{
    public class YandexGamesReviewService : MonoBehaviour, IReview
    {
        bool IInitializable.Initialized => true;

        bool ISelectableService.Available => YG_Sdk.ServicesIsSupported;


        void IInitializable.Initialize() { }

        void IReview.Request()
        {
            YG_Review.Request(
                onOpened: () => TimeMode.SetActive(TimeMode.Mode.Stop, true),
                onClosed: () => TimeMode.SetActive(TimeMode.Mode.Stop, false));
        }

    }
}


