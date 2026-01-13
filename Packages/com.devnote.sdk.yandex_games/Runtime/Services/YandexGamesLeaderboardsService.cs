using UnityEngine;


namespace DevNote.SDK.YandexGames
{
    public class YandexGamesLeaderboardsService : MonoBehaviour, ILeaderboards
    {
        bool ISelectableService.IsAvailableForSelection => YG_Sdk.IsAvailableForSelection;
        bool ILeaderboards.PlatformIsSupportsLeaderboards => false;

        bool IInitializable.Initialized => YG_Sdk.available;

        void IInitializable.Initialize() { }

        void ILeaderboards.OpenLeaderboard(LeaderboardKey leaderboardKey)
        {
            Debug.LogError("[Yandex Games] Leaderboards opening is not supported");
        }

        void ILeaderboards.SetScore(int value, LeaderboardKey leaderboardKey) 
            => YG_Leaderboards.SetLeaderboardScore(leaderboardKey.ToString(), value);



    }

}

