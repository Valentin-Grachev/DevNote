using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using MirraGames.SDK.Common;
using UnityEngine;
using MirraSdk = MirraGames.SDK.MirraSDK;

namespace DevNote.SDK.MirraSDK
{
    public class MirraPurchaseService : MonoBehaviour, IPurchase
    {
        [SerializeField] private ProductConverter _converter;

        private bool _initialized;
        private bool _successFetched;
        private Dictionary<ProductKey, string> _prices;
        private readonly List<ProductKey> _notConsumedProducts = new();

        private readonly Holder<ISave> save = new();

        bool IPurchase.PlatformIsSupportsPurchases => MirraSdk.Payments.IsPaymentsAvailable;

        bool IInitializable.Initialized => _initialized;

        bool ISelectableService.IsAvailableForSelection
            => MirraEnvironmentService.IsAvailableForSelection && !IEnvironment.IsEditor;

        string IPurchase.GetPriceString(ProductKey productKey)
        {
            if (_successFetched && _prices != null && _prices.TryGetValue(productKey, out var price))
                return price;

            return $"${productKey}";
        }

        async void IInitializable.Initialize()
        {
            ISave.OnSavesDeleted += OnSavesDeleted;

            await UniTask.WaitUntil(() => MirraSdk.IsInitialized && save.Item.Initialized);

            bool restoreCompleted = false;
            IRestoreData restoreData = null;

            MirraSdk.Payments.RestorePurchases((data) =>
            {
                restoreData = data;
                restoreCompleted = true;
            });

            await UniTask.WaitUntil(() => restoreCompleted);

            if (restoreData != null)
            {
                bool hasConsumableProduct = false;

                foreach (var productTag in restoreData.PendingProducts)
                {
                    ProductKey productKey = _converter.GetProductKey(productTag);
                    restoreData.RestoreProduct(productTag, () =>
                    {
                        IPurchase.InvokeHandlePurchase(productKey, success: true);

                        if (IPurchaseHandler.ProductIsConsumable(productKey))
                            hasConsumableProduct = true;
                    });
                }

                foreach (var productTag in restoreData.AllPurchases)
                {
                    ProductKey productKey = _converter.GetProductKey(productTag);
                    if (IPurchaseHandler.ProductIsConsumable(productKey))
                        continue;

                    IPurchase.InvokeHandlePurchase(productKey, success: true);

                    if (!_notConsumedProducts.Contains(productKey))
                        _notConsumedProducts.Add(productKey);
                }

                if (hasConsumableProduct)
                    save.Item.FullSave();
            }

            _prices = new Dictionary<ProductKey, string>();
            foreach (var productKey in Utils.GetEnumTypes<ProductKey>())
            {
                string productId = _converter.GetProductId(productKey);
                ProductData productData = MirraSdk.Payments.GetProductData(productId);

                if (productData != null && !string.IsNullOrEmpty(productData.Currency))
                    _prices[productKey] = productData.GetFullPriceInteger();
            }

            _successFetched = true;
            _initialized = true;
        }

        private void OnSavesDeleted()
        {
            foreach (var productKey in _notConsumedProducts)
                MirraSdk.Payments.SetSupplyCount(_converter.GetProductId(productKey), 0);
        }

        void IPurchase.Purchase(ProductKey productKey, Action onSuccess, Action onError)
        {
            if (!_successFetched)
            {
                IPurchase.InvokeHandlePurchase(productKey, false, onSuccess, onError);
                return;
            }

            string productId = _converter.GetProductId(productKey);

            MirraSdk.Payments.Purchase(productId,
                onSuccess: () =>
                {
                    IPurchase.InvokeHandlePurchase(productKey, true, onSuccess, onError);

                    if (IPurchaseHandler.ProductIsConsumable(productKey))
                        MirraSdk.Payments.SupplyProduct(productId, () => { });

                    save.Item.FullSave();
                },
                onError: () =>
                {
                    IPurchase.InvokeHandlePurchase(productKey, false, onSuccess, onError);
                });
        }
    }
}
