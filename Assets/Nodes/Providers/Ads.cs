using System;
using UnityEngine;

namespace Nodes.Providers
{
    public static class Ads
    {
        public enum Status
        {
            Success, NoAdsPurchased, AdBlocked, CooldownNotPassed, NotSupported, Error
        }

        public interface IProvider : ICommonProvider
        {
            public bool IsSupportRewarded { get; }
            public bool IsSupportInterstitial { get; }
            public bool IsAdBlock { get; }

            public void ShowRewarded(Action<bool> onHandled = null);
            public void ShowInterstitial(Action<bool> onHandled = null);
        }


        public delegate void AdEvent(AdKey key, Status status);
        public static event AdEvent OnInterstitialHandled, OnRewardedHandled;

        private static IProvider _provider;
        private static float _adShowUnscaledTime = 0f;
        private static float _interstitialCooldown = 0f;
        private static bool _skipAds = false;
        private static Func<bool> _noAdsIsPurchased;

        public static bool IsInitialized => _provider != null;
        public static bool IsSupportRewarded => _provider.IsSupportRewarded;
        public static bool IsSupportInterstitial => _provider.IsSupportInterstitial;
        public static bool IsAdBlock => _provider.IsAdBlock;



        public static void SetNoAdsIsPurchased(Func<bool> noAdsIsPurchased)
            => _noAdsIsPurchased = noAdsIsPurchased;


        public static void InitializeProvider(IProvider provider, Action onInitialized)
            => provider.Initialize(onInitialized: () =>
            {
                _provider = provider;
                onInitialized?.Invoke();
            });

        public static void SetInterstitialCooldown(float value)
            => _interstitialCooldown = value;

        public static void SetSkipAds(bool value) => _skipAds = value;
        

        public static void ShowRewarded(AdKey key = default, 
            Action onRewarded = null, Action<Status> callback = null)
        {
            if (!_provider.IsSupportRewarded)
                InvokeStatus(Status.NotSupported);

            else if (_skipAds)
                InvokeStatus(Status.Success);

            else _provider.ShowRewarded(onHandled: (success) =>
            {
                if (success) InvokeStatus(Status.Success);
                else if (_provider.IsAdBlock) InvokeStatus(Status.AdBlocked);
                else InvokeStatus(Status.Error);
            });


            void InvokeStatus(Status status)
            {
                if (status == Status.Success)
                {
                    _adShowUnscaledTime = Time.unscaledTime;
                    onRewarded?.Invoke();
                }

                callback?.Invoke(status);
                OnRewardedHandled?.Invoke(key, status);
            }
        }

        public static void ShowInterstitial(AdKey key = default, Action<Status> callback = null)
        {
            bool cooldownIsPassed = Time.unscaledTime - _adShowUnscaledTime > _interstitialCooldown;

            if (!_provider.IsSupportInterstitial)
                InvokeStatus(Status.NotSupported);

            else if (_noAdsIsPurchased != null && _noAdsIsPurchased.Invoke())
                InvokeStatus(Status.NoAdsPurchased);

            else if (!cooldownIsPassed)
                InvokeStatus(Status.CooldownNotPassed);

            else if (_skipAds)
                InvokeStatus(Status.Success);

            else _provider.ShowInterstitial(onHandled: (success) =>
            {
                if (success) InvokeStatus(Status.Success);
                else if (_provider.IsAdBlock) InvokeStatus(Status.AdBlocked);
                else InvokeStatus(Status.Error);
            });

            void InvokeStatus(Status status)
            {
                if (status == Status.Success)
                    _adShowUnscaledTime = Time.unscaledTime;

                callback?.Invoke(status);
                OnInterstitialHandled?.Invoke(key, status);
            }

        }

    }
}


