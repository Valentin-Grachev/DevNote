
namespace DevNote
{
    public interface ILeaderboards : ISelectableService, IInitializable
    {
        
        public bool LeaderboardsIsSupported { get; }

        public void SetScore(int value, string leaderboardKey = "Default");



    }
}

