
namespace DevNote
{
    public interface ILeaderboards : ISelectableService, IInitializable
    {
        
        public bool PlatformIsSupportsLeaderboards { get; }

        public void SetScore(int value, LeaderboardKey leaderboardKey = LeaderboardKey.Main);

        public void OpenLeaderboard(LeaderboardKey leaderboardKey = LeaderboardKey.Main);

    }
}

