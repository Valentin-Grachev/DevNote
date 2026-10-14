#if UNITY_WEBGL
using GamePush;
#endif
using UnityEngine;

namespace DevNote.SDK.GamePush
{
    public class GamePushLeaderboardsService : MonoBehaviour, ILeaderboards
    {
#if UNITY_WEBGL
        bool ILeaderboards.PlatformIsSupportsLeaderboards => true;

        bool ISelectableService.IsAvailableForSelection => GamePushEnvironmentService.IsAvailableForSelection;

        bool IInitializable.Initialized => GP_Init.isReady;

        void IInitializable.Initialize() { }

        void ILeaderboards.OpenLeaderboard(LeaderboardKey leaderboardKey)
        {
            if (leaderboardKey == LeaderboardKey.Main)
                GP_Leaderboard.Open(withMe: WithMe.last);

            else Debug.LogError($"Leaderboard \"{leaderboardKey}\" doesn't have realization");
        }

        void ILeaderboards.SetScore(int value, LeaderboardKey leaderboardKey)
        {
            if (leaderboardKey == LeaderboardKey.Main)
                GP_Player.SetScore(value);

            else Debug.LogError($"Leaderboard \"{leaderboardKey}\" doesn't have realization");
        }
#else
        bool ILeaderboards.PlatformIsSupportsLeaderboards => false;

        bool ISelectableService.IsAvailableForSelection => false;

        bool IInitializable.Initialized => false;

        void IInitializable.Initialize() { }

        void ILeaderboards.OpenLeaderboard(LeaderboardKey leaderboardKey) { }

        void ILeaderboards.SetScore(int value, LeaderboardKey leaderboardKey) { }
#endif
    }

}
