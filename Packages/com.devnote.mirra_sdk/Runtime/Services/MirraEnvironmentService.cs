using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using MirraLanguageType = MirraGames.SDK.Common.LanguageType;
using MirraPlatformType = MirraGames.SDK.Common.PlatformType;
using MirraSdk = MirraGames.SDK.MirraSDK;

namespace DevNote.SDK.MirraSDK
{
    public class MirraEnvironmentService : MonoBehaviour, IEnvironment
    {
        private bool _initialized = false; bool IInitializable.Initialized => _initialized;
        private bool _gameplayIsStarted = false;

        private readonly List<MirraPlatformType> INVITE_PLATFORMS = new()
        {
            MirraPlatformType.OK,
            MirraPlatformType.VK,
            MirraPlatformType.Telegram,
            MirraPlatformType.Facebook,
        };


        public static bool IsAvailableForSelection
            => IEnvironment.EnvironmentKey == EnvironmentKey.MirraSDK;

        Language IEnvironment.DeviceLanguage => MirraSdk.Language.Current switch
        {
            MirraLanguageType.English => Language.EN,
            MirraLanguageType.Russian => Language.RU,
            _ => Language.EN,
        };

        DeviceType IEnvironment.DeviceType => MirraSdk.Device.IsMobile ? 
            DeviceType.Mobile : DeviceType.Desktop;


        float IEnvironment.TimeScale
        {
            get => MirraSdk.Time.Scale;
            set => MirraSdk.Time.Scale = value;
        }


        bool ISelectableService.IsAvailableForSelection => IsAvailableForSelection;

        bool IEnvironment.FullscreenIsSupported => false;

        bool IEnvironment.IsFullscreen => false;

        bool IEnvironment.InviteAvailable => INVITE_PLATFORMS.Contains(MirraSdk.Platform.Current);

        void IEnvironment.GameReady() => MirraSdk.Analytics.GameIsReady();

        async void IInitializable.Initialize()
        {
            await UniTask.WaitUntil(() => MirraSdk.IsInitialized);

            IEnvironment.StartGameUtcTime = MirraSdk.Time.CurrentDate;
            IEnvironment.GameStoreName = MirraSdk.Platform.Current.ToString().ToLower();

            _initialized = true;
        }

        void IEnvironment.OpenURL(string url) => MirraSdk.Device.OpenUrl(url);

        void IEnvironment.StartGameplay()
        {
            if (!_gameplayIsStarted)
            {
                _gameplayIsStarted = true;
                MirraSdk.Analytics.GameplayStart();
            }
        }

        void IEnvironment.StopGameplay()
        {
            if (_gameplayIsStarted)
            {
                _gameplayIsStarted = false;
                MirraSdk.Analytics.GameplayStop();
            }      
        }

        bool IEnvironment.ChannelIsMuted(Sound.Channel channel)
            => channel == Sound.Channel.Music ? !Sound.Settings.MusicEnabled : !Sound.Settings.SfxEnabled;

        void IEnvironment.SetChannelMute(Sound.Channel channel, bool isMute)
        {
            if (channel == Sound.Channel.Music)
                Sound.Settings.MusicEnabled = !isMute;

            if (channel == Sound.Channel.SFX)
                Sound.Settings.SfxEnabled = !isMute;

            IEnvironment.InvokeChangeSoundChannel();
        }

        void IEnvironment.ToggleFullscreen() { }

        void IEnvironment.Invite() => MirraSdk.Platform.ShareGame();
    }
}
