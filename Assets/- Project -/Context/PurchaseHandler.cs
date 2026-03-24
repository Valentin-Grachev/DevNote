using System.Collections.Generic;
using DevNote;
using UnityEngine;

public class PurchaseHandler : MonoBehaviour, IPurchaseHandler
{
    List<ProductKey> IPurchaseHandler.PermanentProducts => new() 
    { 
        ProductKey.NoAds
    };


    void IPurchaseHandler.HandlePurchase(ProductKey productKey)
    {
        switch (productKey)
        {
            case ProductKey.NoAds: break;

            default:
                Debug.LogWarning($"Handle for product {productKey} does not exist!");
                break;
        }
    }

}

