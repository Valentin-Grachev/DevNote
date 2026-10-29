using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using MirraSdk = MirraGames.SDK.MirraSDK;

namespace DevNote.SDK.MirraSDK
{
    public class MirraAdsService : MonoBehaviour, IAds
    {
        [SerializeField] private bool _showInterstitialOnStart = true;
        [SerializeField] private float _interstitialCooldown = 60f;


        bool ISelectableService.IsAvailableForSelection => 
            MirraEnvironmentService.IsAvailableForSelection;

        bool IAds.RewardedAvailable => MirraSdk.Ads.IsRewardedAvailable;

        bool IAds.InterstitialAvailable => IAds.InterstitialCooldownPassed
            && MirraSdk.Ads.IsInterstitialAvailable && !IAds.SkipAds;

        bool IAds.AdBlockEnabled => false;

        bool IInitializable.Initialized => MirraSdk.IsInitialized;

        async void IInitializable.Initialize()
        {
            IAds.InterstitialCooldown = _interstitialCooldown;

            await UniTask.WaitUntil(() => MirraSdk.IsInitialized);

            if (_showInterstitialOnStart)
                ShowInterstitial(AdKey.Inter_Start, callback: null);

            if (MirraSdk.Ads.IsBannerAvailable)
            {
                if (IGameState.NoAdsPurchased) MirraSdk.Ads.DisableBanner();
                else MirraSdk.Ads.InvokeBanner();
            }
        }

        void IAds.SetBanner(bool active)
        {
            if (active)
            {
                if (MirraSdk.Ads.IsBannerAvailable)
                    MirraSdk.Ads.InvokeBanner();
            }
            else
            {
                MirraSdk.Ads.DisableBanner();
            }
        }

        void IAds.ShowInterstitial(AdKey key, Action<AdShowStatus> callback)
            => ShowInterstitial(key, callback);

        private void ShowInterstitial(AdKey key, Action<AdShowStatus> callback)
        {
            if (IAds.TryInternalHandleInterstitial(callback, key))
                return;

            if (MirraSdk.Ads.IsInterstitialAvailable)
            {
                MirraSdk.Ads.InvokeInterstitial(onClose: (success) =>
                {
                    var status = success ? AdShowStatus.Success : AdShowStatus.Error;
                    IAds.InvokeInterstitialCallback(callback, key, status);
                });
            }
            else
            {
                IAds.InvokeInterstitialCallback(callback, key, AdShowStatus.Error);
            }
        }


        void IAds.ShowRewarded(AdKey key, Action onRewarded, Action<AdShowStatus> callback)
        {
            if (IAds.TryInternalHandleRewarded(onRewarded, callback, key))
                return;

            if (MirraSdk.Ads.IsRewardedAvailable)
            {
                MirraSdk.Ads.InvokeRewarded(
                    onClose: (success) =>
                    {
                        var status = success ? AdShowStatus.Success : AdShowStatus.Error;
                        IAds.InvokeRewardedCallback(onRewarded, callback, key, status);
                    },
                    rewardTag: key.ToString());
            }
            else
            {
                IAds.InvokeRewardedCallback(onRewarded, callback, key, AdShowStatus.Error);
            }
        }
    }
}
