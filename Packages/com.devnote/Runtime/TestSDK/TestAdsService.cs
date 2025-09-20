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

        void IAds.ShowInterstitial(string key, Action<AdShowStatus> callback)
        {
            var status = IAds.InterstitialCooldownPassed ? AdShowStatus.Success : AdShowStatus.CooldownNotFinished;

            Debug.Log($"{Info.Prefix} Show intertstitial. Key: \"{key}\", Status: {status}");
            IAds.InvokeInterstitialCallback(callback, key, status);
        }

        void IAds.ShowRewarded(string key, Action onRewarded, Action<AdShowStatus> callback)
        {
            var status = IAds.InterstitialCooldownPassed ? AdShowStatus.Success : AdShowStatus.CooldownNotFinished;

            Debug.Log($"{Info.Prefix} Show rewarded. Key: \"{key}\", Status: {status}");
            IAds.InvokeRewardedCallback(onRewarded, callback, key, status);
        }


    }
}

