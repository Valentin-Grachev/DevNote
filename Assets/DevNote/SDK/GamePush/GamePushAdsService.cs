using System;
using GamePush;
using UnityEngine;


namespace DevNote.Services.GamePush
{
    public class GamePushAdsService : MonoBehaviour, IAds
    {
        bool ISelectableService.IsAvailableForSelection => GamePushEnvironmentService.ServicesIsAvailable;
        bool IInitializable.Initialized => GP_Init.isReady;

        bool IAds.InterstitialAvailable => GP_Ads.IsFullscreenAvailable() && IAds.InterstitialCooldownPassed;
        bool IAds.RewardedAvailable => GP_Ads.IsRewardedAvailable();
        bool IAds.AdBlockEnabled => GP_Ads.IsAdblockEnabled();

        void IInitializable.Initialize() { }


        void IAds.SetBanner(bool active)
        {
            if (active) GP_Ads.ShowSticky();
            else GP_Ads.CloseSticky();
        }

        void IAds.ShowRewarded(AdKey key, Action onRewarded, Action<AdShowStatus> callback)
        {
            if (IAds.SkipAds)
                IAds.InvokeRewardedCallback(onRewarded, callback, key, AdShowStatus.Success);

            else if (GP_Ads.IsRewardedAvailable())
            {
                GP_Ads.ShowRewarded(key.ToString(),
                    onRewardedStart: () => TimeMode.SetActive(TimeMode.Mode.Stop, true),
                    onRewardedClose: (success) =>
                    {
                        TimeMode.SetActive(TimeMode.Mode.Stop, false);

                        AdShowStatus status = success ? AdShowStatus.Success : AdShowStatus.Error;
                        IAds.InvokeRewardedCallback(onRewarded, callback, key, status);
                    });
            }
            else
            {
                AdShowStatus status = GP_Ads.IsAdblockEnabled() ? AdShowStatus.AdBlockEnabled : AdShowStatus.Error;
                IAds.InvokeRewardedCallback(onRewarded, callback, key, status);
            }
        }

        void IAds.ShowInterstitial(AdKey key, Action<AdShowStatus> callback)
        {
            if (IAds.SkipAds)
                IAds.InvokeInterstitialCallback(callback, key, AdShowStatus.Success);

            else if (GameState.NoAdsPurchased.Value)
                IAds.InvokeInterstitialCallback(callback, key, AdShowStatus.NoAdsPurchased);

            else if (!IAds.InterstitialCooldownPassed)
                IAds.InvokeInterstitialCallback(callback, key, AdShowStatus.CooldownNotFinished);

            else if (GP_Ads.IsFullscreenAvailable())
            {
                GP_Ads.ShowFullscreen(
                onFullscreenStart: () => TimeMode.SetActive(TimeMode.Mode.Stop, true),
                onFullscreenClose: (success) =>
                {
                    TimeMode.SetActive(TimeMode.Mode.Stop, false);

                    AdShowStatus status = success ? AdShowStatus.Success : AdShowStatus.Error;
                    IAds.InvokeInterstitialCallback(callback, key, status);
                });
            }
            else
            {
                AdShowStatus status = GP_Ads.IsAdblockEnabled() ? AdShowStatus.AdBlockEnabled : AdShowStatus.Error;
                IAds.InvokeInterstitialCallback(callback, key, status);
            }
        }
        



    }
}


