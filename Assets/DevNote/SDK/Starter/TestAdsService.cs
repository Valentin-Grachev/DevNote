using System;
using UnityEngine;

namespace DevNote.Services.Test
{
    public class TestAdsService : MonoBehaviour, IAds
    {
        public event IAds.AdShownEvent onInterstitialShown;
        public event IAds.AdShownEvent onRewardedShown;


        bool ISelectableService.Available => true;

        bool IProjectInitializable.Initialized => true;

        

        void IProjectInitializable.Initialize() { }

        void IAds.SetBanner(bool active) {}

        void IAds.ShowInterstitial(AdKey adKey, Action onShown, Action onError)
        {
            onShown?.Invoke();
            onInterstitialShown?.Invoke(adKey, true);
        }

        void IAds.ShowRewarded(AdKey adKey, Action onRewarded, Action onError)
        {
            onRewarded?.Invoke();
            onRewardedShown?.Invoke(adKey, true);
        }
    }
}

