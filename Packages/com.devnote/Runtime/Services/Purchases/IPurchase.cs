using System;
using UnityEngine;

namespace DevNote
{
    public interface IPurchase : IInitializable, ISelectableService
    {
        public delegate void OnPurchaseHandle(ProductKey productKey, bool success);
        public static event OnPurchaseHandle OnPurchaseHandled;


        public bool PlatformIsSupportsPurchases { get; }

        public string GetPriceString(ProductKey productKey);
        public void Purchase(ProductKey productKey, Action onSuccess = null, Action onError = null);


        public static void InvokeHandlePurchase(ProductKey productKey, bool success, Action onSuccess = null, Action onError = null)
        {
            bool isConsumable = IPurchaseHandler.ProductIsConsumable(productKey);
            bool permanentProductIsPurchased = IGameState.PurchasedPermanentProducts.Contains(productKey);

            if (isConsumable || !permanentProductIsPurchased)
            {
                if (success)
                {
                    if (!isConsumable)
                        IGameState.PurchasedPermanentProducts.Add(productKey);
                        
                    IPurchaseHandler.HandlePurchaseStatic(productKey);
                    onSuccess?.Invoke();
                }
                else onError?.Invoke();

                OnPurchaseHandled?.Invoke(productKey, success);
            }
        }
    }

}
