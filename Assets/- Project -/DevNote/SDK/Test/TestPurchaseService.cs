using System;
using UnityEngine;

namespace DevNote.Services.Test
{
    public class TestPurchaseService : MonoBehaviour, IPurchase
    {
        public event IPurchase.OnPurchaseHandle OnPurchaseHandled;


        bool IInitializable.Initialized => true;

        bool ISelectableService.IsAvailableForSelection => true;

        

        string IPurchase.GetPriceString(string productKey) => $"${productKey}";

        void IInitializable.Initialize() { }

        void IPurchase.Purchase(string productKey, Action onSuccess, Action onError)
        {
            PurchaseHandler.HandlePurchase(productKey);
            OnPurchaseHandled?.Invoke(productKey, true);
            onSuccess?.Invoke();
        }
    }
}



