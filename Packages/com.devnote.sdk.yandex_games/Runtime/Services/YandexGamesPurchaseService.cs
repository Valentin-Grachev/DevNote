using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DevNote.SDK.YandexGames
{
    public class YandexGamesPurchaseService : MonoBehaviour, IPurchase
    {
        private bool _initialized = false;
        private List<string> _purchasedProductKeys;
        private Dictionary<ProductKey, string> _productPrices;

        private readonly Holder<ISave> save = new();

        bool ISelectableService.IsAvailableForSelection => YG_Sdk.IsAvailableForSelection;
        bool IInitializable.Initialized => _initialized;

        bool IPurchase.PlatformIsSupportsPurchases => true;

        async void IInitializable.Initialize()
        {
            await UniTask.WaitUntil(() => YG_Purchases.available && save.Item.Initialized);

            ISave.OnSavesDeleted += OnSavesDeleted;

            YG_Purchases.InitializePayments();

            YG_Purchases.GetPurchasedProducts((purchasedProductKeys) =>
            {
                _purchasedProductKeys = purchasedProductKeys;
                foreach (var purchasedProductKeyString in _purchasedProductKeys)
                {
                    if (purchasedProductKeyString == string.Empty)
                        continue;

                    var purchasedProductKey = purchasedProductKeyString.ToEnum<ProductKey>();

                    IPurchase.InvokeHandlePurchase(purchasedProductKey, success: true);

                    if (IPurchaseHandler.ProductIsConsumable(purchasedProductKey))
                        YG_Purchases.Consume(purchasedProductKeyString);
                }
            });

            YG_Purchases.GetPrices((productPrices) =>
            {
                _productPrices = new();
                foreach (var productPrice in productPrices)
                    _productPrices.Add(productPrice.Key.ToEnum<ProductKey>(), productPrice.Value);


            });

            await UniTask.WaitUntil(() => _purchasedProductKeys != null && _productPrices != null);
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
            YG_Purchases.Purchase(productKey.ToString(), onPurchasedSuccess: (success) =>
            {
                if (success)
                {
                    if (IPurchaseHandler.ProductIsConsumable(productKey))
                        YG_Purchases.Consume(productKey.ToString());
                }

                IPurchase.InvokeHandlePurchase(productKey, success, onSuccess, onError);
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


