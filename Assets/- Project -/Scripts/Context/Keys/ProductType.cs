using System.Collections.Generic;

namespace DevNote
{
    public enum ProductType
    {
        None = 0,
        NoAds = 1,
    }

    public static class ProductTypeExtensions
    {
        private static readonly Dictionary<ProductType, bool> isConsumableProducts = new()
        {
            { ProductType.NoAds, false },
        };

        public static bool IsConsumable(this ProductType productType) 
            => isConsumableProducts[productType];


    }


}



