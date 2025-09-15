using UnityEngine;


namespace DevNote.YandexGamesSDK
{
    public class YandexGamesLeaderboardsService : MonoBehaviour, ILeaderboards
    {
        bool ISelectableService.IsAvailableForSelection => YG_Sdk.ServicesIsSupported;
        bool ILeaderboards.LeaderboardsIsSupported => true;

        bool IInitializable.Initialized => YG_Sdk.available;

        void IInitializable.Initialize() { }

        void ILeaderboards.SetScore(LeaderboardType leaderboardType, int value) 
            => YG_Leaderboards.SetLeaderboardScore(leaderboardType.ToString(), value);



    }

}

