using System;
using UnityEngine;

namespace DevNote.SDK.Test
{
    public class TestAdsService : MonoBehaviour, IAds
    {
        bool ISelectableService.IsAvailableForSelection => true;
        bool IInitializable.Initialized => true;

        bool IAds.RewardedAvailable => true;
        bool IAds.InterstitialAvailable => IAds.InterstitialCooldownPassed;
        bool IAds.AdBlockEnabled => false;


        void IInitializable.Initialize() { }


        void IAds.SetBanner(bool active)
        {
            Debug.Log($"{Info.Prefix} Set banner {active}");
        }

        void IAds.ShowInterstitial(AdKey key, Action<AdShowStatus> callback)
        {
            var status = AdShowStatus.Success;
            if (IGameState.NoAdsPurchased.Value) status = AdShowStatus.NoAdsPurchased;
            else if (!IAds.InterstitialCooldownPassed) status = AdShowStatus.CooldownNotFinished;

            Debug.Log($"{Info.Prefix} Show intertstitial. Key: \"{key}\", Status: {status}");
            IAds.InvokeInterstitialCallback(callback, key, status);
        }

        void IAds.ShowRewarded(AdKey key, Action onRewarded, Action<AdShowStatus> callback)
        {
            var status = AdShowStatus.Success;

            Debug.Log($"{Info.Prefix} Show rewarded. Key: \"{key}\", Status: {status}");
            IAds.InvokeRewardedCallback(onRewarded, callback, key, status);
        }


    }
}

