using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DevNote.YandexGamesSDK;
using UnityEngine;

namespace DevNote.Services.YandexGames
{
    public class YandexGamesPurchaseService : MonoBehaviour, IPurchase
    {
        public event IPurchase.OnPurchaseHandle OnPurchaseHandled;

        [SerializeField] private ProductIdConvertor _productConvertor;

        private bool _initialized = false;
        private List<string> _purchasedProductIds;
        private Dictionary<string, string> _productPrices;

        private readonly Holder<ISave> save = new();

        bool ISelectableService.IsAvailableForSelection => YG_Sdk.ServicesIsSupported;
        bool IInitializable.Initialized => _initialized;

        async void IInitializable.Initialize()
        {
            await UniTask.WaitUntil(() => YG_Purchases.available && save.Item.Initialized);

            ISave.OnSavesDeleted += OnSavesDeleted;

            YG_Purchases.InitializePayments();

            YG_Purchases.GetPurchasedProducts((purchasedProductIds) =>
            {
                _purchasedProductIds = purchasedProductIds;
                foreach (var purchasedProductId in _purchasedProductIds)
                {
                    if (purchasedProductId == string.Empty)
                        continue;

                    var productType = _productConvertor.GetProductKey(purchasedProductId);
                    PurchaseHandler.HandlePurchase(productType);

                    if (productType.IsConsumable())
                        YG_Purchases.Consume(purchasedProductId);
                }
            });

            YG_Purchases.GetPrices((productPrices) => _productPrices = productPrices);

            await UniTask.WaitUntil(() => _purchasedProductIds != null && _productPrices != null);
            _initialized = true;
        }


        string IPurchase.GetPriceString(string productKey)
        {
            string productId = _productConvertor.GetProductId(productKey);

            if (!_productPrices.ContainsKey(productId))
                return string.Empty;

            return _productPrices[productId];
        }

        void IPurchase.Purchase(string productKey, Action onSuccess, Action onError)
        {
            string productId = _productConvertor.GetProductId(productKey);

            YG_Purchases.Purchase(productId, onPurchasedSuccess: (success) =>
            {
                if (success)
                {
                    PurchaseHandler.HandlePurchase(productKey);

                    if (productKey.IsConsumable())
                        YG_Purchases.Consume(productId);

                    onSuccess?.Invoke();
                    OnPurchaseHandled?.Invoke(productKey, success: true);
                }
                else
                {
                    onError?.Invoke();
                    OnPurchaseHandled?.Invoke(productKey, success: false);
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


