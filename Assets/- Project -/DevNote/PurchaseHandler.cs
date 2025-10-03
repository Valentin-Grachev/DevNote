using System.Collections.Generic;
using DevNote;
using UnityEngine;

public class PurchaseHandler : MonoBehaviour, IPurchaseHandler
{
    List<string> IPurchaseHandler.ConsumableProductKeys => new();


    void IPurchaseHandler.HandlePurchase(string productKey)
    {
        switch (productKey)
        {
            case ProductKey.NoAds:
                IGameState.NoAdsPurchased.Value = true;
                break;

            default:
                Debug.LogWarning($"Handle for product {productKey} does not exist!");
                break;
        }
    }
}

