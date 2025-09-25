

public class EnvironmentKey : DevNote.IEnvironmentKey, DevNote.SDK.YandexGames.IEnvironmentKey
{
    public const string Test = DevNote.IEnvironmentKey.Test;
    public const string YandexGames = DevNote.SDK.YandexGames.IEnvironmentKey.YandexGames;

}

public class AdKey : DevNote.IAdKey
{
    public const string Default = DevNote.IAdKey.Default;

}

public class TableKey : DevNote.ITableKey
{
    public const string Localization = DevNote.ITableKey.Localization;

}

public class LeaderboardKey : DevNote.ILeaderboardKey
{
    public const string Default = DevNote.ILeaderboardKey.Default;

}

public class ProductKey : DevNote.IProductKey
{
    public const string NoAds = DevNote.IProductKey.NoAds;

}

