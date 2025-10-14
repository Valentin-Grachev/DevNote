using System;
using GamePush;
using UnityEngine;

namespace DevNote.SDK.GamePush
{
    public class GamePushAdsService : MonoBehaviour, IAds
    {
        bool ISelectableService.IsAvailableForSelection => GamePushEnvironmentService.IsAvailableForSelection;

        bool IAds.RewardedAvailable => GP_Ads.IsRewardedAvailable();

        bool IAds.InterstitialAvailable => IAds.InterstitialCooldownPassed && GP_Ads.IsFullscreenAvailable();

        bool IAds.AdBlockEnabled => GP_Ads.IsAdblockEnabled();

        bool IInitializable.Initialized => GP_Init.isReady;


        void IInitializable.Initialize() { }

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
            if (IAds.TryInternalHandleRewarded(onRewarded, callback, key)) return;

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

