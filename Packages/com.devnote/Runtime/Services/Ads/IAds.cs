using System;
using UnityEngine;

namespace DevNote
{
    public partial interface IAds : IInitializable, ISelectableService // Interface
    {
        public bool RewardedAvailable { get; }
        public bool InterstitialAvailable { get; }
        public bool AdBlockEnabled { get; }


        public void ShowRewarded(string key = "Default", Action onRewarded = null, Action<AdShowStatus> callback = null);
        public void ShowInterstitial(string key = "Default", Action<AdShowStatus> callback = null);
        public void SetBanner(bool active);

    }

    public partial interface IAds // Handlers
    {
        public delegate void OnAdShow(string key, AdShowStatus status);
        public static event OnAdShow OnInterstitialShown, OnRewardedShown;

        public static bool SkipAds { get; set; } = false;
        public static float InterstitialCooldown { get; set; } = 0f;


        public static float InterstitialShowLastTime { get; private set; } = 0f;
        protected static bool InterstitialCooldownPassed => Time.unscaledTime - InterstitialShowLastTime > InterstitialCooldown;


        protected static void InvokeInterstitialCallback(Action<AdShowStatus> callback, string key, AdShowStatus status)
        {
            if (status == AdShowStatus.Success)
                InterstitialShowLastTime = Time.unscaledTime;

            callback?.Invoke(status);
            OnInterstitialShown?.Invoke(key, status);
        }

        protected static void InvokeRewardedCallback(Action onRewarded, Action<AdShowStatus> callback, string key, AdShowStatus status)
        {
            if (status == AdShowStatus.Success)
                onRewarded?.Invoke();

            callback?.Invoke(status);
            OnRewardedShown?.Invoke(key, status);
        }
    }



}
