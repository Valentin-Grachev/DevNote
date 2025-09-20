using System;
using UnityEngine;


namespace DevNote.SDK.YandexGames
{
    public class YandexGamesAdsService : MonoBehaviour, IAds
    {
        bool ISelectableService.IsAvailableForSelection => YG_Sdk.ServicesIsSupported;
        bool IInitializable.Initialized => YG_Sdk.available;

        bool IAds.RewardedAvailable => true;
        bool IAds.InterstitialAvailable => IAds.InterstitialCooldownPassed;
        bool IAds.AdBlockEnabled => false;


        void IInitializable.Initialize() { }

        void IAds.SetBanner(bool active)
        {
            if (active) YG_Ads.ShowBanner();
            else YG_Ads.HideBanner();
        }

        void IAds.ShowRewarded(string key, Action onRewarded, Action<AdShowStatus> callback)
        {
            if (IAds.SkipAds)
                IAds.InvokeRewardedCallback(onRewarded, callback, key, AdShowStatus.Success);

            else
            {
                bool rewarded = false;
                bool error = false;

                YG_Ads.ShowRewarded((action) =>
                {
                    switch (action)
                    {
                        case YG_Ads.RewardedAction.Opened:
                            TimeMode.SetActive(TimeMode.Mode.Stop, true);
                            break;

                        case YG_Ads.RewardedAction.Failed:
                            error = true;
                            break;

                        case YG_Ads.RewardedAction.Rewarded:
                            rewarded = true;
                            break;

                        case YG_Ads.RewardedAction.Closed:
                            TimeMode.SetActive(TimeMode.Mode.Stop, false);

                            var status = !error && rewarded ? AdShowStatus.Success : AdShowStatus.Error;
                            IAds.InvokeRewardedCallback(onRewarded, callback, key, status);

                            break;
                    }

                });
            }
        }

        void IAds.ShowInterstitial(string key, Action<AdShowStatus> callback)
        {
            if (IAds.SkipAds)
                IAds.InvokeInterstitialCallback(callback, key, AdShowStatus.Success);

            else if (IGameState.NoAdsPurchased.Value)
                IAds.InvokeInterstitialCallback(callback, key, AdShowStatus.NoAdsPurchased);

            else if (!IAds.InterstitialCooldownPassed)
                IAds.InvokeInterstitialCallback(callback, key, AdShowStatus.CooldownNotFinished);

            else
            {
                bool interstitialWasShown = false;
                TimeMode.SetActive(TimeMode.Mode.Stop, true);

                YG_Ads.ShowInterstitial((action) =>
                {
                    switch (action)
                    {
                        case YG_Ads.InterstitialAction.Opened:
                            interstitialWasShown = true;
                            break;

                        case YG_Ads.InterstitialAction.Closed:
                            TimeMode.SetActive(TimeMode.Mode.Stop, false);

                            var status = interstitialWasShown ? AdShowStatus.Success : AdShowStatus.Error;
                            IAds.InvokeInterstitialCallback(callback, key, status);

                            break;
                    }

                });
            }
        }
    }
}



