using System;

namespace DevNote
{
    public interface IPurchase : IInitializable, ISelectableService
    {
        public delegate void OnPurchaseHandle(ProductKey productKey, bool success);
        public static event OnPurchaseHandle OnPurchaseHandled;

        public string GetPriceString(ProductKey productKey);
        public void Purchase(ProductKey productKey, Action onSuccess = null, Action onError = null);


        public static void InvokeHandlePurchaseCallback(ProductKey productKey, bool success, Action onSuccess = null, Action onError = null)
        {
            if (success) IPurchaseHandler.HandlePurchaseStatic(productKey);

            if (success) onSuccess?.Invoke();
            else onError?.Invoke();

            OnPurchaseHandled?.Invoke(productKey, success);
        }
    }

}
