using System;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace DevNote.SDK.YandexGames
{
    public class YandexGamesAdsService : MonoBehaviour, IAds
    {
        [SerializeField] private bool _noAdsDisableBanner = true;

        private readonly Holder<ISave> save = new();

        bool ISelectableService.IsAvailableForSelection => YG_Sdk.IsAvailableForSelection;
        bool IInitializable.Initialized => YG_Sdk.available;

        bool IAds.RewardedAvailable => true;
        bool IAds.InterstitialAvailable => IAds.InterstitialCooldownPassed && !IAds.SkipAds;
        bool IAds.AdBlockEnabled => false;


        async void IInitializable.Initialize() 
        {
            IAds.InterstitialCooldown = 60f;
            IPurchase.OnPurchaseHandled += OnPurchaseHandled;

            await UniTask.WaitUntil(() => YG_Sdk.available && save.Item.Initialized);
            ApplyBanner(true);
        }

        private void OnDestroy() => IPurchase.OnPurchaseHandled -= OnPurchaseHandled;

        void IAds.SetBanner(bool active) => ApplyBanner(active);

        private void OnPurchaseHandled(ProductKey productKey, bool success)
        {
            if (success && productKey == ProductKey.NoAds && _noAdsDisableBanner)
                YG_Ads.HideBanner();
        }

        private void ApplyBanner(bool active)
        {
            if (_noAdsDisableBanner && IGameState.NoAdsPurchased)
                active = false;

            if (active) YG_Ads.ShowBanner();
            else YG_Ads.HideBanner();
        }

        void IAds.ShowRewarded(AdKey key, Action onRewarded, Action<AdShowStatus> callback)
        {
            if (IAds.TryInternalHandleRewarded(onRewarded, callback, key)) return;

            bool rewarded = false;
            bool error = false;

            IAds.RewardedInProcess = true;

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
                        IAds.RewardedInProcess = false;
                        TimeMode.SetActive(TimeMode.Mode.Stop, false);

                        var status = !error && rewarded ? AdShowStatus.Success : AdShowStatus.Error;
                        IAds.InvokeRewardedCallback(onRewarded, callback, key, status);

                        break;
                }

            });
        }

        void IAds.ShowInterstitial(AdKey key, Action<AdShowStatus> callback)
        {
            if (IAds.TryInternalHandleInterstitial(callback, key)) return;


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



