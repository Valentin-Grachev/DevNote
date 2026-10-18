using UnityEngine;
using MirraSdk = MirraGames.SDK.MirraSDK;

namespace DevNote.SDK.MirraSDK
{
    public class MirraLeaderboardsService : MonoBehaviour, ILeaderboards
    {
        bool ILeaderboards.PlatformIsSupportsLeaderboards => true;

        bool ISelectableService.IsAvailableForSelection => MirraEnvironmentService.IsAvailableForSelection;

        bool IInitializable.Initialized => MirraSdk.IsInitialized;

        void IInitializable.Initialize() { }

        void ILeaderboards.OpenLeaderboard(LeaderboardKey leaderboardKey)
        {
            Debug.LogWarning($"[{nameof(MirraLeaderboardsService)}] OpenLeaderboard is not supported by MirraSDK");
        }

        void ILeaderboards.SetScore(int value, LeaderboardKey leaderboardKey)
        {
            MirraSdk.Achievements.SetScore(leaderboardKey.ToString(), value);
        }
    }
}
