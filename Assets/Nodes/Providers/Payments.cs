using System;
using System.Collections.Generic;

namespace Nodes.Providers
{
    public static class Payments
    {
        public interface IProvider : ICommonProvider
        {
            public bool IsSupportPayments { get; }

            public string GetPriceText(ProductKey productKey);
            public void Purchase(ProductKey productKey, Action<bool> onHandled);
            public List<ProductKey> GetNonHandledProducts();
        }

        public delegate void PaymentEvent(ProductKey productKey, bool success);
        public static event PaymentEvent OnPaymentHandled;


        private static IProvider _provider;

        public static void InitializeProvider(IProvider provider, Action onInitialized)
            => provider.Initialize(onInitialized: () =>
            {
                _provider = provider;

                foreach (var productKey in _provider.GetNonHandledProducts())
                    OnPaymentHandled?.Invoke(productKey, success: true);

                onInitialized?.Invoke();
            });

        public static bool IsInitialized => _provider != null;
        public static bool IsSupportPayments => _provider.IsSupportPayments;

        public static string GetPriceText(ProductKey productKey) 
            => _provider.GetPriceText(productKey);

        public static void Purchase(ProductKey productKey, Action<bool> onHandled = null)
        {
            _provider.Purchase(productKey, onHandled: (success) =>
            {
                onHandled?.Invoke(success);
                OnPaymentHandled?.Invoke(productKey, success);
            });
        }

    }
}



