using UnityEngine;



namespace DevNote.SDK.YandexGames
{
    public class YandexGamesReviewService : MonoBehaviour, IReview
    {
        bool IInitializable.Initialized => true;

        bool ISelectableService.IsAvailableForSelection => YG_Sdk.ServicesIsSupported;


        void IInitializable.Initialize() { }

        void IReview.Rate()
        {
            YG_Review.Request(
                onOpened: () => TimeMode.SetActive(TimeMode.Mode.Stop, true),
                onClosed: () => TimeMode.SetActive(TimeMode.Mode.Stop, false));
        }

    }
}


