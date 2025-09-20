using DevNote;


public class EnvironmentKey : IEnvironmentKey
{
    public const string Test = IEnvironmentKey.Test;
    public const string YandexGames = IEnvironmentKey.YandexGames;

}

public class AdKey : IAdKey
{
    public const string Default = IAdKey.Default;

}

public class TableKey : ITableKey
{
    public const string Localization = ITableKey.Localization;

}

public class LeaderboardKey : ILeaderboardKey
{
    public const string Default = ILeaderboardKey.Default;

}

public class ProductKey : IProductKey
{
    public const string NoAds = IProductKey.NoAds;

}

