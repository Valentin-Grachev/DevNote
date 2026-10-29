using System;
using System.Collections.Generic;
using UnityEngine;


namespace Nodes.Providers.Test
{
    public class TestPaymentsProvider : MonoBehaviour, Payments.IProvider
    {
        [SerializeField] private bool _isSupportPayments = true;
        [SerializeField] private bool _simulatePaymentsError = false;
        [SerializeField] private List<ProductKey> _nonHandledProducts;


        bool Payments.IProvider.IsSupportPayments => _isSupportPayments;

        List<ProductKey> Payments.IProvider.GetNonHandledProducts() 
            => _nonHandledProducts;

        string Payments.IProvider.GetPriceText(ProductKey productKey)
            => $"${productKey}";

        void ICommonProvider.Initialize(Action onInitialized) => onInitialized?.Invoke();

        void Payments.IProvider.Purchase(ProductKey productKey, Action<bool> onHandled)
        {
            if (!_simulatePaymentsError)
                onHandled?.Invoke(true);

            else
            {
                Debug.Log("[Payments] Simulating payment error.");
                onHandled?.Invoke(false);
            }
        }
    }
}

