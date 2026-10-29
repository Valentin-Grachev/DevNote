using System;
using UnityEngine;

namespace Nodes.Providers.Test
{
    public class TestAdsProvider : MonoBehaviour, Ads.IProvider
    {
        [SerializeField] private bool _isSupportRewarded = true;
        [SerializeField] private bool _isSupportInterstitial = true;
        [SerializeField] private bool _isAdBlock = false;
        [SerializeField] private bool _showLogs = true;
        [SerializeField] private bool _simulateAdError = false;



        bool Ads.IProvider.IsSupportRewarded => _isSupportRewarded;
        bool Ads.IProvider.IsSupportInterstitial => _isSupportInterstitial;
        bool Ads.IProvider.IsAdBlock => _isAdBlock;


        void ICommonProvider.Initialize(Action onInitialized) => onInitialized?.Invoke();
        void Ads.IProvider.ShowInterstitial(Action<bool> onHandled) 
        { 
            bool success = !_simulateAdError;

            if (_showLogs)
            {
                string result = success ? "SUCCESS" : "ERROR";
                Debug.Log($"[Ads] Show Interstitial: {result}");
            }

            onHandled?.Invoke(success);
        }
        void Ads.IProvider.ShowRewarded(Action<bool> onHandled) 
        {
            bool success = !_simulateAdError;

            if (_showLogs)
            {
                string result = success ? "SUCCESS" : "ERROR";
                Debug.Log($"[Ads] Show Rewarded: {result}");
            }

            onHandled?.Invoke(success);
        }

    }
}

