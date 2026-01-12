using System;
using Cysharp.Threading.Tasks;
using GamePush;
using UnityEngine;

namespace DevNote.SDK.GamePush
{
    public class GamePushAdsService : MonoBehaviour, IAds
    {
        [SerializeField] private float _interstitialCooldown = 60f;
        [SerializeField] private bool _noAdsDisableBanner = true;

        private readonly Holder<ISave> save = new();

        bool ISelectableService.IsAvailableForSelection => GamePushEnvironmentService.IsAvailableForSelection;

        bool IAds.RewardedAvailable => GP_Ads.IsRewardedAvailable() || IEnvironment.IsEditor;

        bool IAds.InterstitialAvailable => IAds.InterstitialCooldownPassed && GP_Ads.IsFullscreenAvailable();

        bool IAds.AdBlockEnabled => GP_Ads.IsAdblockEnabled();

        bool IInitializable.Initialized => GP_Init.isReady;


        async void IInitializable.Initialize() 
        {
            IAds.InterstitialCooldown = _interstitialCooldown;

            await UniTask.WaitUntil(() => GP_Init.isReady && save.Item.Initialized);

            if (!IGameState.NoAdsPurchased.Value)
            {
                if (GP_Ads.IsStickyAvailable()) GP_Ads.ShowSticky();
                if (GP_Ads.IsPreloaderAvailable()) GP_Ads.ShowPreloader();
            }
            else if (!_noAdsDisableBanner && GP_Ads.IsStickyAvailable())
                GP_Ads.ShowSticky();

        }

        void IAds.SetBanner(bool active)
        {
            if (active) GP_Ads.ShowSticky();
            else GP_Ads.CloseSticky();
        }

        void IAds.ShowInterstitial(AdKey key, Action<AdShowStatus> callback)
        {
            if (IAds.TryInternalHandleInterstitial(callback, key)) return;

            if (GP_Ads.IsFullscreenAvailable())
            {
                GP_Ads.ShowFullscreen(onFullscreenClose: (success) =>
                {
                    var status = success ? AdShowStatus.Success : AdShowStatus.Error;
                    IAds.InvokeInterstitialCallback(callback, key, status);
                });
            }
            else
            {
                var status = GP_Ads.IsAdblockEnabled() ? AdShowStatus.AdBlockEnabled : AdShowStatus.Error;
                IAds.InvokeInterstitialCallback(callback, key, status);
            }
        }

        void IAds.ShowRewarded(AdKey key, Action onRewarded, Action<AdShowStatus> callback)
        {
            if (IEnvironment.IsEditor) 
                IAds.InvokeRewardedCallback(onRewarded, callback, key, AdShowStatus.Success);

            else if (IAds.TryInternalHandleRewarded(onRewarded, callback, key)) 
                return;

            else if (GP_Ads.IsRewardedAvailable())
            {
                GP_Ads.ShowRewarded(key.ToString(), onRewardedReward: (id) =>
                {
                    IAds.InvokeRewardedCallback(onRewarded, callback, key, AdShowStatus.Success);
                },
                onRewardedClose: (success) =>
                {
                    if (!success)
                        IAds.InvokeRewardedCallback(onRewarded, callback, key, AdShowStatus.Error);
                });
            }
            else
            {
                var status = GP_Ads.IsAdblockEnabled() ? AdShowStatus.AdBlockEnabled : AdShowStatus.Error;
                IAds.InvokeRewardedCallback(onRewarded, callback, key, status);
            }
        }
    }
}

