using UnityEngine;

namespace DevNote
{
    public static class PurchaseHandler
    {
        public static void HandlePurchase(string productKey)
        {
            
            switch (productKey)
            {
                case ProductKey.NoAds:
                    GameState.NoAdsPurchased.Value = true;
                    break;

                default: 
                    Debug.LogWarning($"Handle for product {productKey} does not exist!");
                    break;
            }
            
            
        }


    }
}

