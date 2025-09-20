using System;
using System.Runtime.InteropServices;
using UnityEngine;



namespace DevNote.SDK.YandexGames
{
    public class YG_Sdk : MonoBehaviour
    {

        public static bool ServicesIsSupported =>
            IEnvironment.IsEditor == false &&
            IEnvironment.PlatformType == PlatformType.WebGL &&
            IEnvironment.EnvironmentKey == IEnvironmentKey.YandexGames;


        private void Awake()
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }



        private static bool _sdkInitialized = false;
        public static bool available 
        { 
            get 
            {
#if UNITY_WEBGL
                CheckSdkInit();
#endif
                return _sdkInitialized;
            } 
        }

        private void HTML_OnSdkInitChecked(int initialized) => _sdkInitialized = Convert.ToBoolean(initialized);


        public static string GetLanguage()
        {
#if UNITY_WEBGL
            RequestLanguage();
#endif
            return _receivedLanguage;
        }

        private static string _receivedLanguage;
        private void HTML_OnLanguageReceived(string language) => _receivedLanguage = language;


        private static DeviceType _receivedDeviceType;
        private void HTML_OnDeviceTypeReceived(string deviceType)
        {
            if (deviceType == "desktop") _receivedDeviceType = DeviceType.Desktop;
            else _receivedDeviceType = DeviceType.Mobile;
        }
        public static DeviceType GetDeviceType()
        {
#if UNITY_WEBGL
            _GetDeviceType();
#endif
            return _receivedDeviceType;
        }




#if UNITY_WEBGL
        [DllImport("__Internal")] private static extern string RequestLanguage();
        [DllImport("__Internal")] private static extern void CheckSdkInit();
        [DllImport("__Internal")] private static extern void _GetDeviceType();
#endif

    }
}



