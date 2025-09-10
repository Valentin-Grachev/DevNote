using System;

namespace DevNote
{
    public interface IPurchase : IInitializable, ISelectableService
    {
#pragma warning disable CS0067
        public delegate void OnPurchaseHandle(ProductType productType, bool success);
        public static event OnPurchaseHandle OnPurchaseHandled;
#pragma warning restore CS0067

        public string GetPriceString(ProductType productType);
        public void Purchase(ProductType productType, Action onSuccess = null, Action onError = null);
    }

}
