using UnityEngine;


namespace DevNote.SDK.YandexGames
{
    public class YandexGamesLeaderboardsService : MonoBehaviour, ILeaderboards
    {
        bool ISelectableService.IsAvailableForSelection => YG_Sdk.IsAvailableForSelection;
        bool ILeaderboards.LeaderboardsIsSupported => true;

        bool IInitializable.Initialized => YG_Sdk.available;

        void IInitializable.Initialize() { }

        void ILeaderboards.SetScore(int value, string leaderboardKey) 
            => YG_Leaderboards.SetLeaderboardScore(leaderboardKey.ToString(), value);



    }

}

