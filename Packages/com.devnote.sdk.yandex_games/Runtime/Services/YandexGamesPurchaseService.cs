using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DevNote.SDK.YandexGames
{
    public class YandexGamesPurchaseService : MonoBehaviour, IPurchase
    {
        [SerializeField] private ProductConverter _converter;

        private bool _initialized = false;
        private Dictionary<ProductKey, string> _productPrices;

        private readonly Holder<ISave> save = new();

        bool ISelectableService.IsAvailableForSelection => YG_Sdk.IsAvailableForSelection;
        bool IInitializable.Initialized => _initialized;

        bool IPurchase.PlatformIsSupportsPurchases => true;

        async void IInitializable.Initialize()
        {
            List<string> purchasedProductIds = null;

            await UniTask.WaitUntil(() => YG_Purchases.available && save.Item.Initialized);

            ISave.OnSavesDeleted += OnSavesDeleted;

            YG_Purchases.InitializePayments();

            YG_Purchases.GetPurchasedProducts((productIds) =>
            {
                purchasedProductIds = productIds;

                bool hasConsumableProduct = false;

                foreach (var purchasedProductId in purchasedProductIds)
                {
                    if (purchasedProductId == string.Empty)
                        continue;

                    var purchasedProductKey = _converter.GetProductKey(purchasedProductId);

                    IPurchase.InvokeHandlePurchase(purchasedProductKey, success: true);

                    if (IPurchaseHandler.ProductIsConsumable(purchasedProductKey))
                    {
                        YG_Purchases.Consume(purchasedProductId);
                        hasConsumableProduct = true;
                    }  
                }

                if (hasConsumableProduct) save.Item.FullSave();
            });

            YG_Purchases.GetPrices((productPrices) =>
            {
                _productPrices = new();
                foreach (var productPrice in productPrices)
                {
                    var productId = productPrice.Key;
                    var productKey = _converter.GetProductKey(productId);
                    _productPrices.Add(productKey, productPrice.Value);
                }
            });

            await UniTask.WaitUntil(() => purchasedProductIds != null && _productPrices != null);
            _initialized = true;
        }


        string IPurchase.GetPriceString(ProductKey productKey)
        {
            if (!_productPrices.ContainsKey(productKey))
                return string.Empty;

            return _productPrices[productKey];
        }

        void IPurchase.Purchase(ProductKey productKey, Action onSuccess, Action onError)
        {
            string productId = _converter.GetProductId(productKey);

            YG_Purchases.Purchase(productId, onPurchasedSuccess: (success) =>
            {
                if (success)
                {
                    if (IPurchaseHandler.ProductIsConsumable(productKey))
                        YG_Purchases.Consume(productKey.ToString());
                }

                IPurchase.InvokeHandlePurchase(productKey, success, onSuccess, onError);

                if (success) save.Item.FullSave();
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


