using System;
using UnityEngine;

namespace Nodes.Providers.Test
{
    public class TestAdsProvider : MonoBehaviour, Ads.IProvider
    {
        [SerializeField] private bool _isSupportRewarded = true;
        [SerializeField] private bool _isSupportInterstitial = true;
        [SerializeField] private bool _isAdBlock = false;


        bool Ads.IProvider.IsSupportRewarded => _isSupportRewarded;
        bool Ads.IProvider.IsSupportInterstitial => _isSupportInterstitial;
        bool Ads.IProvider.IsAdBlock => _isAdBlock;


        void ICommonProvider.Initialize(Action onInitialized) => onInitialized?.Invoke();
        void Ads.IProvider.ShowInterstitial(Action<bool> onHandled) { }
        void Ads.IProvider.ShowRewarded(Action<bool> onHandled) { }

    }
}

