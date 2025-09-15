using System;
using UnityEngine;

namespace DevNote.Services.Test
{
    public class TestPurchaseService : MonoBehaviour, IPurchase
    {
        public event IPurchase.OnPurchaseHandle OnPurchaseHandled;


        bool IInitializable.Initialized => true;

        bool ISelectableService.IsAvailableForSelection => true;

        

        string IPurchase.GetPriceString(ProductType productType) => $"${productType}";

        void IInitializable.Initialize() { }

        void IPurchase.Purchase(ProductType productType, Action onSuccess, Action onError)
        {
            PurchaseHandler.HandlePurchase(productType);
            OnPurchaseHandled?.Invoke(productType, true);
            onSuccess?.Invoke();
        }
    }
}



