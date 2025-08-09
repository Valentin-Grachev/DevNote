using System;
using UnityEngine;

namespace DevNote
{
    public partial interface IAds : IInitializable, ISelectableService // Interface
    {
        public bool RewardedAvailable { get; }
        public bool InterstitialAvailable { get; }
        public bool AdBlockEnabled { get; }


        public void ShowRewarded(AdKey key, Action onRewarded = null, Action<AdShowStatus> callback = null);
        public void ShowInterstitial(AdKey key, Action<AdShowStatus> callback = null);
        public void SetBanner(bool active);

    }

    public partial interface IAds // Handlers
    {
        public delegate void OnAdShow(AdKey key, AdShowStatus status);
        public static event OnAdShow OnInterstitialShown;
        public static event OnAdShow OnRewardedShown;

        public static bool SkipAds { get; set; } = false;
        public static float InterstitialCooldown { get; set; } = 0f;


        private static float _interstitialShowLastTime = 0f;
        protected static bool InterstitialCooldownPassed => Time.time - _interstitialShowLastTime > InterstitialCooldown;


        protected static void InvokeInterstitialCallback(Action<AdShowStatus> callback, AdKey key, AdShowStatus status)
        {
            if (status == AdShowStatus.Success)
                _interstitialShowLastTime = Time.time;

            callback?.Invoke(status);
            OnInterstitialShown?.Invoke(key, status);
        }

        protected static void InvokeRewardedCallback(Action onRewarded, Action<AdShowStatus> callback, AdKey key, AdShowStatus status)
        {
            if (status == AdShowStatus.Success)
                onRewarded?.Invoke();

            callback?.Invoke(status);
            OnRewardedShown?.Invoke(key, status);
        }
    }



}
