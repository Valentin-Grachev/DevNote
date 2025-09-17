using System.Collections.Generic;

namespace DevNote
{
    public static partial class ProductKey
    {



        private static readonly List<string> consumableProductKeys = new()
        {
            
        };


        public static bool IsConsumable(this string productKey) 
            => consumableProductKeys.Contains(productKey);

    }
}

