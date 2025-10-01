using System;

namespace DevNote
{
    public interface IEnvironment : IInitializable, ISelectableService
    {
        public static bool IsTest { get; set; }
        public static string EnvironmentKey { get; set; }

        protected static DateTime StartGameTime { get; set; }

        public static DateTime Time => StartGameTime.AddSeconds(UnityEngine.Time.realtimeSinceStartup);

        public static PlatformType PlatformType
        {
            get
            {
                #if UNITY_WEBGL
                    return PlatformType.WebGL;
                #endif

                #if UNITY_ANDROID
                    return PlatformType.Android;
                #endif

                #if UNITY_STANDALONE
                    return PlatformType.Desktop;
                #endif

                #if UNITY_IOS
                    return PlatformType.iOS;
                #endif
            }
        }

        public static bool IsEditor
        {
            get
            {
                #if UNITY_EDITOR
                    return true;
                #else
                    return false;
                #endif
            }
        }


        public Language DeviceLanguage { get; }
        public DeviceType DeviceType { get; }
        public void GameReady();
        public void OpenURL(string url);
        

    }

}

