using System;
using UnityEngine;

namespace DevNote.SDK.Test
{
    public class TestPurchaseService : MonoBehaviour, IPurchase
    {
        bool IInitializable.Initialized => true;

        bool ISelectableService.IsAvailableForSelection => true;

        

        string IPurchase.GetPriceString(string productKey) => $"${productKey}";

        void IInitializable.Initialize() { }

        void IPurchase.Purchase(string productKey, Action onSuccess, Action onError)
        {
            IPurchase.InvokeHandlePurchaseCallback(productKey, success: true, onSuccess, onError);
            onSuccess?.Invoke();
        }
    }
}



