using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GamePush;
using UnityEngine;

namespace DevNote.SDK.GamePush
{
    public class GamePushPurchaseService : MonoBehaviour, IPurchase
    {
        [SerializeField] private ProductConverter _converter;


        private bool _initialized = false;
        private bool _successFetched = false;
        private Dictionary<ProductKey, string> _prices;
        private List<ProductKey> _notConsumedProducts = new();

        private readonly Holder<ISave> save = new();
        private readonly Holder<IEnvironment> environment = new();

        bool IPurchase.PlatformIsSupportsPurchases => GP_Payments.IsPaymentsAvailable();

        bool IInitializable.Initialized => _initialized;

        bool ISelectableService.IsAvailableForSelection 
            => GamePushEnvironmentService.IsAvailableForSelection && !IEnvironment.IsEditor;


        string IPurchase.GetPriceString(ProductKey productKey) 
            => GP_Payments.IsPaymentsAvailable() && _successFetched ? _prices[productKey] : $"${productKey}";
                


        async void IInitializable.Initialize()
        {
            ISave.OnSavesDeleted += OnSavesDeleted;

            await UniTask.WaitUntil(() => GP_Init.isReady && save.Item.Initialized);

            bool productsFetchSuccess = false;
            bool purchasesFetchSuccess = false;
            bool isError = false;

            GP_Payments.Fetch(onFetchProducts: (productList) =>
            {
                _prices = new();

                foreach (var product in productList)
                {
                    ProductKey productKey = _converter.GetProductKey(product.tag);
                    _prices.Add(productKey, $"{product.price} {product.currencySymbol}");
                }
                productsFetchSuccess = true;
            },
            onFetchPlayerPurchases: (playerPurchasesList) =>
            {
                bool hasConsumableProduct = false;
                foreach (var playerPurchase in playerPurchasesList)
                {
                    ProductKey productKey = _converter.GetProductKey(playerPurchase.tag);

                    IPurchase.InvokeHandlePurchase(productKey, success: true);

                    if (IPurchaseHandler.ProductIsConsumable(productKey))
                    {
                        GP_Payments.Consume(_converter.GetProductId(productKey));
                        hasConsumableProduct = true;
                    }
                    else _notConsumedProducts.Add(productKey);
                }

                if (hasConsumableProduct) save.Item.FullSave();

                purchasesFetchSuccess = true;
            }, 
            onFetchProductsError: () => isError = true);

            await UniTask.WaitUntil(() => productsFetchSuccess && purchasesFetchSuccess || isError);

            _successFetched = !isError;
            _initialized = true;
        }

        private void OnSavesDeleted()
        {
            foreach (var productKey in _notConsumedProducts)
                GP_Payments.Consume(_converter.GetProductId(productKey));
        }

        void IPurchase.Purchase(ProductKey productKey, Action onSuccess, Action onError)
        {
            if (!_successFetched) IPurchase.InvokeHandlePurchase(productKey, false, onSuccess, onError);

            else if (GP_Payments.IsPaymentsAvailable())
            {
                if (environment.Item.FullscreenIsSupported && environment.Item.IsFullscreen) 
                    environment.Item.ToggleFullscreen();

                GP_Payments.Purchase(_converter.GetProductId(productKey), onPurchaseSuccess: (key) =>
                {
                    IPurchase.InvokeHandlePurchase(productKey, true, onSuccess, onError);

                    if (IPurchaseHandler.ProductIsConsumable(productKey))
                        GP_Payments.Consume(_converter.GetProductId(productKey));

                    save.Item.FullSave();
                },
                onPurchaseError: () =>
                {
                    IPurchase.InvokeHandlePurchase(productKey, false, onSuccess, onError);
                });

            }
            else IPurchase.InvokeHandlePurchase(productKey, false, onSuccess, onError);
        }


    }
}

