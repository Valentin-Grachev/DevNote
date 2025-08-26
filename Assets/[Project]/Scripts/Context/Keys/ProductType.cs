using System.Collections.Generic;

namespace DevNote
{
    public enum ProductType
    {
        None = 0,
        NoAds = 1,
    }

    public static class ProductCatalog
    {
        private static readonly Dictionary<ProductType, bool> isConsumableProducts = new Dictionary<ProductType, bool>
        {
            { ProductType.NoAds, false },
        };

        public static bool IsConsumable(ProductType productType) => isConsumableProducts[productType];


    }


}



