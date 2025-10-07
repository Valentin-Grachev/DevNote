using UnityEngine;

namespace DevNote.SDK.Test
{
    public class TestLeaderboardsService : MonoBehaviour, ILeaderboards
    {
        bool ILeaderboards.LeaderboardsIsSupported => true;

        bool ISelectableService.IsAvailableForSelection => true;

        bool IInitializable.Initialized => true;

        void IInitializable.Initialize() { }

        void ILeaderboards.SetScore(int value, LeaderboardKey leaderboardKey)
        {
            Debug.Log($"{Info.Prefix} Leaderboard \"{leaderboardKey}\": Set score {value}");
        }
    }
}

