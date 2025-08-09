using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GamePush;
using UnityEngine;

namespace DevNote.Services.GamePush
{
    public class GamePushPurchaseService : MonoBehaviour, IPurchase
    {
        public event IPurchase.OnPurchaseHandled onPurchaseHandled;

        [SerializeField] private ProductIdConvertor _productConvertor;

        private readonly Holder<ISave> save = new();

        private bool _initialized = false;
        private Dictionary<ProductType, string> _productPrices = new();

        bool ISelectableService.Available => GamePushEnvironmentService.ServicesIsAvailable && !IEnvironment.IsEditor;


        bool IInitializable.Initialized => _initialized;
        async void IInitializable.Initialize() 
        {
            await UniTask.WaitUntil(() => GP_Init.isReady);

            if (GP_Payments.IsPaymentsAvailable())
            {
                GP_Payments.Fetch(
                onFetchProducts: (products) =>
                {
                    foreach (var product in products)
                    {
                        ProductType productType = GetProductTypeById(product.id);
                        string price = $"{product.price} {product.currencySymbol}";
                        _productPrices.Add(productType, price);
                    }
                },
                onFetchPlayerPurchases: async (purchases) =>
                {
                    await UniTask.WaitUntil(() => save.Item.Initialized);

                    foreach (var purchase in purchases)
                    {
                        var purchasedProduct = GetProductTypeById(purchase.productId);

                        if (ProductCatalog.IsConsumable(purchasedProduct))
                        {
                            int id = GetProductIdByType(purchasedProduct);

                            PurchaseHandler.HandlePurchase(purchasedProduct);
                            GP_Payments.Consume(id.ToString());

                            _initialized = true;
                        }

                        else
                        {
                            PurchaseHandler.HandlePurchase(purchasedProduct);
                            _initialized = true;
                        }
                    }
                },

                onFetchProductsError: () => _initialized = true);
            }

            else _initialized = true;
        }



        private ProductType GetProductTypeById(int id) => _productConvertor.GetProductType(id.ToString());
        private int GetProductIdByType(ProductType productType) => int.Parse(_productConvertor.GetProductId(productType));


        string IPurchase.GetPriceString(ProductType productType) => _productPrices[productType];


        void IPurchase.Purchase(ProductType productType, Action onSuccess, Action onError)
        {
            int productId = GetProductIdByType(productType);
            GP_Payments.Purchase(productId.ToString(),
            onPurchaseSuccess: (key) =>
            {
                PurchaseHandler.HandlePurchase(productType);
                GP_Payments.Consume(productId.ToString());

                onSuccess?.Invoke();
                onPurchaseHandled?.Invoke(productType, true);
            },
            onPurchaseError: () =>
            {
                onError?.Invoke();
                onPurchaseHandled?.Invoke(productType, false);
            });
        }
    }

}

