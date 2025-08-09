using System;

namespace DevNote
{
    public interface IEnvironment : IInitializable, ISelectableService
    {
        public static bool IsTest { get; set; }
        public static EnvironmentType EnvironmentType { get; set; }
        public static bool ShowAds { get; set; } = true;

        


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

        public DateTime ServerTime { get; }
        public Language CurrentLanguage { get; }
        public DeviceType DeviceType { get; }
        public void GameReady();

    }

}

