using System;
using UnityEngine;

#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif




namespace DevNote.SDK.YandexGames
{
    public class YG_Ads : MonoBehaviour
    {
        public enum RewardedAction { Opened, Closed, Rewarded, Failed };
        public enum InterstitialAction { Opened, Closed, Failed };




        public static void ShowRewarded(Action<RewardedAction> onAction)
        {
            _onRewardedAction = onAction;

#if UNITY_WEBGL
            ShowRewarded();
#endif
        }


        public static void ShowInterstitial(Action<InterstitialAction> onAction)
        {
            _onInterstitialAction = onAction;

#if UNITY_WEBGL
            ShowInterstitial();
#endif

        }





        #region Internal

        private static event Action<RewardedAction> _onRewardedAction;
        
        public void HTML_OnRewardedAction(string actionTypeString)
        {
            Enum.TryParse(actionTypeString, out RewardedAction actionType);
            _onRewardedAction?.Invoke(actionType);
        }



        private static event Action<InterstitialAction> _onInterstitialAction;
        
        public void HTML_OnInterstitialAction(string actionTypeString)
        {
            Enum.TryParse(actionTypeString, out InterstitialAction actionType);
            _onInterstitialAction?.Invoke(actionType);
        }


        #endregion

        public static void ShowBanner()
        {
#if UNITY_WEBGL
            _ShowBanner();
#endif
        }

        public static void HideBanner()
        {
#if UNITY_WEBGL
            _HideBanner();
#endif
        }


#if UNITY_WEBGL

        [DllImport("__Internal")] public static extern void _ShowBanner();
        [DllImport("__Internal")] public static extern void _HideBanner();
        [DllImport("__Internal")] private static extern void ShowRewarded();
        [DllImport("__Internal")] private static extern void ShowInterstitial();

#endif


    }
}



