using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DevNote.YandexGamesSDK;
using UnityEngine;
using VContainer;

namespace DevNote.Services.YandexGames
{
    public class YandexGamesPurchaseService : MonoBehaviour, IPurchase
    {
        public event IPurchase.OnPurchaseHandled onPurchaseHandled;

        [SerializeField] private ProductIdConvertor _productConvertor;

        private bool _initialized = false;
        private List<string> _purchasedProductIds;
        private Dictionary<string, string> _productPrices;

        [Inject] private readonly ISave save;

        bool ISelectableService.Available => YG_Sdk.ServicesIsSupported;
        bool IProjectInitializable.Initialized => _initialized;

        async void IProjectInitializable.Initialize()
        {
            await UniTask.WaitUntil(() => YG_Purchases.available && save.Initialized);

            save.onSavesDeleted += OnSavesDeleted;

            YG_Purchases.InitializePayments();

            YG_Purchases.GetPurchasedProducts((purchasedProductIds) =>
            {
                _purchasedProductIds = purchasedProductIds;
                foreach (var purchasedProductId in _purchasedProductIds)
                {
                    if (purchasedProductId == string.Empty)
                        continue;

                    var productType = _productConvertor.GetProductType(purchasedProductId);
                    PurchaseHandler.HandlePurchase(productType);

                    if (ProductCatalog.IsConsumable(productType))
                        YG_Purchases.Consume(purchasedProductId);
                }
            });

            YG_Purchases.GetPrices((productPrices) => _productPrices = productPrices);

            await UniTask.WaitUntil(() => _purchasedProductIds != null && _productPrices != null);
            _initialized = true;
        }


        string IPurchase.GetPriceString(ProductType productType)
        {
            string productId = _productConvertor.GetProductId(productType);

            if (!_productPrices.ContainsKey(productId))
                return string.Empty;

            return _productPrices[productId];
        }

        void IPurchase.Purchase(ProductType productType, Action onSuccess, Action onError)
        {
            string productId = _productConvertor.GetProductId(productType);

            YG_Purchases.Purchase(productId, onPurchasedSuccess: (success) =>
            {
                if (success)
                {
                    PurchaseHandler.HandlePurchase(productType);

                    if (ProductCatalog.IsConsumable(productType))
                        YG_Purchases.Consume(productId);

                    onSuccess?.Invoke();
                    onPurchaseHandled?.Invoke(productType, success: true);
                }
                else
                {
                    onError?.Invoke();
                    onPurchaseHandled?.Invoke(productType, success: false);
                }
            });
            
        }


        private void OnSavesDeleted()
        {
            YG_Purchases.GetPurchasedProducts((purchasedProductIds) =>
            {
                foreach (var purchasedProductKey in purchasedProductIds)
                    YG_Purchases.Consume(purchasedProductKey);
            });
        }

        
    }
}


