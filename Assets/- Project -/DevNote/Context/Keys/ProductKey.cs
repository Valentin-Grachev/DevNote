using System.Collections.Generic;

public static partial class ProductKey
{
    public const string NoAds = nameof(NoAds);



    private static readonly List<string> consumableProductKeys = new()
    {

    };


    public static bool IsConsumable(this string productKey)
        => consumableProductKeys.Contains(productKey);

}
