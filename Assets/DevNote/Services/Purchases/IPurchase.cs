using System;

namespace DevNote
{
    public interface IPurchase : IInitializable, ISelectableService
    {
#pragma warning disable CS0067
        public delegate void OnPurchaseHandle(string productKey, bool success);
        public static event OnPurchaseHandle OnPurchaseHandled;
#pragma warning restore CS0067

        public string GetPriceString(string productKey);
        public void Purchase(string productKey, Action onSuccess = null, Action onError = null);
    }

}
