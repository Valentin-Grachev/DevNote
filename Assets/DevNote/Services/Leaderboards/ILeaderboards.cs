
namespace DevNote
{
    public interface ILeaderboards : ISelectableService, IInitializable
    {
        
        public bool LeaderboardsIsSupported { get; }

        public void SetScore(LeaderboardType leaderboardType, int value);



    }
}

