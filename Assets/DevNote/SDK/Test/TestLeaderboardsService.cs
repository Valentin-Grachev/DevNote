using UnityEngine;

namespace DevNote.Services
{
    public class TestLeaderboardsService : MonoBehaviour, ILeaderboards
    {
        bool ILeaderboards.LeaderboardsIsSupported => true;

        bool ISelectableService.IsAvailableForSelection => true;

        bool IInitializable.Initialized => true;

        void IInitializable.Initialize() { }

        void ILeaderboards.SetScore(LeaderboardType leaderboardType, int value)
        {
            Debug.Log($"{Info.Prefix} Leaderboard \"{leaderboardType}\": Set score {value}");
        }
    }
}

