using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GamePush;
using UnityEngine;

namespace DevNote.SDK.GamePush
{
    public class GamePushPurchaseService : MonoBehaviour, IPurchase
    {
        private bool _initialized = false;
        private bool _successFetched = false;
        private Dictionary<ProductKey, string> _prices;

        bool IPurchase.PlatformIsSupportsPurchases => GP_Payments.IsPaymentsAvailable();

        bool IInitializable.Initialized => _initialized;

        bool ISelectableService.IsAvailableForSelection 
            => GamePushEnvironmentService.IsAvailableForSelection && !IEnvironment.IsEditor;


        string IPurchase.GetPriceString(ProductKey productKey) 
            => GP_Payments.IsPaymentsAvailable() && _successFetched ? _prices[productKey] : $"${productKey}";
                


        async void IInitializable.Initialize()
        {
            await UniTask.WaitUntil(() => GP_Init.isReady);

            bool productsFetchSuccess = false;
            bool purchasesFetchSuccess = false;
            bool isError = false;

            GP_Payments.Fetch(onFetchProducts: (productList) =>
            {
                _prices = new();

                foreach (var product in productList)
                {
                    ProductKey productKey = product.tag.ToEnum<ProductKey>();
                    _prices.Add(productKey, $"{product.price} {product.currencySymbol}");
                }
                productsFetchSuccess = true;
            },
            onFetchPlayerPurchases: (playerPurchasesList) =>
            {
                foreach (var playerPurchase in playerPurchasesList)
                {
                    ProductKey productKey = playerPurchase.tag.ToEnum<ProductKey>();

                    if (IPurchaseHandler.ProductIsConsumable(productKey))
                        GP_Payments.Consume(productKey.ToString());
                }
                purchasesFetchSuccess = true;
            }, 
            onFetchProductsError: () => isError = true);

            await UniTask.WaitUntil(() => productsFetchSuccess && purchasesFetchSuccess || isError);

            _successFetched = !isError;
            _initialized = true;
        }

        void IPurchase.Purchase(ProductKey productKey, Action onSuccess, Action onError)
        {
            if (!_successFetched) IPurchase.InvokeHandlePurchase(productKey, false, onSuccess, onError);

            else if (GP_Payments.IsPaymentsAvailable())
            {
                GP_Payments.Purchase(productKey.ToString(), onPurchaseSuccess: (key) =>
                {
                    IPurchase.InvokeHandlePurchase(productKey, true, onSuccess, onError);

                    if (IPurchaseHandler.ProductIsConsumable(productKey))
                        GP_Payments.Consume(productKey.ToString());
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

