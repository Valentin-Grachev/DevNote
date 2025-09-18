using System.Collections.Generic;


public static partial class EnvironmentKey
{
    public const string YandexGames = nameof(YandexGames);

}

public static class AdKey
{

}

public static class TableKey
{

}

public static class LeaderboardKey
{

}

public static class ProductKey
{
    public const string NoAds = nameof(NoAds);



    private static readonly List<string> consumableProductKeys = new() 
    { 

    };
    public static bool IsConsumable(this string productKey) => consumableProductKeys.Contains(productKey);
}

