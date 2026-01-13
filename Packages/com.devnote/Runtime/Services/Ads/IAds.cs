using System;
using UnityEngine;

namespace DevNote
{
    public interface IAds : IInitializable, ISelectableService
    {
        public bool RewardedAvailable { get; }
        public bool InterstitialAvailable { get; }
        public bool AdBlockEnabled { get; }


        public void ShowRewarded(AdKey key = AdKey.Default, Action onRewarded = null, Action<AdShowStatus> callback = null);
        public void ShowInterstitial(AdKey key = AdKey.Default, Action<AdShowStatus> callback = null);
        public void SetBanner(bool active);





        public delegate void OnAdShow(AdKey key, AdShowStatus status);
        public static event OnAdShow OnInterstitialShown, OnRewardedShown;

        public static bool SkipAds { get; set; } = false;
        public static float InterstitialCooldown { get; set; } = 0f;


        public static float AdShowLastTime { get; private set; } = 0f;
        protected static bool InterstitialCooldownPassed => Time.unscaledTime - AdShowLastTime > InterstitialCooldown;


        protected static void InvokeInterstitialCallback(Action<AdShowStatus> callback, AdKey key, AdShowStatus status)
        {
            if (status == AdShowStatus.Success)
                AdShowLastTime = Time.unscaledTime;

            OnInterstitialShown?.Invoke(key, status);
            callback?.Invoke(status);
        }

        protected static void InvokeRewardedCallback(Action onRewarded, Action<AdShowStatus> callback, AdKey key, AdShowStatus status)
        {
            if (status == AdShowStatus.Success)
            {
                AdShowLastTime = Time.unscaledTime;
                onRewarded?.Invoke();
            }

            OnRewardedShown?.Invoke(key, status);
            callback?.Invoke(status);
        }

        protected static bool TryInternalHandleRewarded(Action onRewarded, Action<AdShowStatus> callback, AdKey key)
        {
            if (SkipAds)
            {
                InvokeRewardedCallback(onRewarded, callback, key, AdShowStatus.Success);
                return true;
            }

            return false;
        }


        protected static bool TryInternalHandleInterstitial(Action<AdShowStatus> callback, AdKey key)
        {
            if (SkipAds)
            {
                InvokeInterstitialCallback(callback, key, AdShowStatus.Success);
                return true;
            }
            else if (IGameState.NoAdsPurchased.Value)
            {
                InvokeInterstitialCallback(callback, key, AdShowStatus.NoAdsPurchased);
                return true;
            }
            else if (!InterstitialCooldownPassed)
            {
                InvokeInterstitialCallback(callback, key, AdShowStatus.CooldownNotFinished);
                return true;
            }
                
            return false;
        }






    }



}
