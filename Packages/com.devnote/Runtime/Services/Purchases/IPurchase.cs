using System;

namespace DevNote
{
    public interface IPurchase : IInitializable, ISelectableService
    {
        public delegate void OnPurchaseHandle(string productKey, bool success);
        public static event OnPurchaseHandle OnPurchaseHandled;

        public string GetPriceString(string productKey);
        public void Purchase(string productKey, Action onSuccess = null, Action onError = null);


        public static void InvokeHandlePurchaseCallback(string productKey, bool success, Action onSuccess = null, Action onError = null)
        {
            if (success) IPurchaseHandler.HandlePurchaseStatic(productKey);

            if (success) onSuccess?.Invoke();
            else onError?.Invoke();

            OnPurchaseHandled?.Invoke(productKey, success);
        }
    }

}
