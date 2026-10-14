#if UNITY_WEBGL
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GamePush;
using GamePush.Initialization;
#endif
using UnityEngine;


namespace DevNote.SDK.GamePush
{
    public class GamePushEnvironmentService : MonoBehaviour, IEnvironment
    {
#if UNITY_WEBGL
        private readonly List<Platform> DISTRIBUTIONS_SUPPORTS_FULLSCREEN = new()
        {
            Platform.OK,
            Platform.VK,
        };

        bool IEnvironment.FullscreenIsSupported =>
            DISTRIBUTIONS_SUPPORTS_FULLSCREEN.Contains(GP_Platform.Type());




        private bool _initialized = false;
        private bool _gameplayStarted = false;



        public static bool IsAvailableForSelection 
            => IEnvironment.EnvironmentKey == EnvironmentKey.GamePush && !IEnvironment.IsEditor;

        Language IEnvironment.DeviceLanguage
        {
            get
            {
                var platformType = GP_Platform.Type();

                return GP_Language.Current() switch
                {
                    global::GamePush.Language.English => Language.EN,
                    global::GamePush.Language.Russian => Language.RU,

                    _ => Language.EN,
                };
            }

        }
        

        DeviceType IEnvironment.DeviceType => GP_Device.IsMobile() ? DeviceType.Mobile : DeviceType.Desktop;

        bool IInitializable.Initialized => _initialized;

        bool ISelectableService.IsAvailableForSelection => IsAvailableForSelection;

        

        bool IEnvironment.IsFullscreen => GP_Fullscreen.IsEnabled();

        bool IEnvironment.InviteAvailable => GP_Socials.IsSupportsNativeInvite();

        void IEnvironment.GameReady() => GP_Game.GameReady();


        async void IInitializable.Initialize()
        {
            GP_Initialization.Execute();

            await UniTask.WaitUntil(() => GP_Init.isReady && Sound.Initialized);

            IEnvironment.StartGameUtcTime = GP_Server.Time();
            IEnvironment.GameStoreName = GP_Platform.Type().ToString().ToLower();

            GP_Sounds.OnMuteSFX += () =>
            {
                AudioListener.pause = true;
            };
            GP_Sounds.OnUnmuteSFX += () =>
            {
                AudioListener.pause = false;
            };
            
            GP_Fullscreen.OnFullscreenChange += () => IEnvironment.InvokeChangeFullscreen();

            _initialized = true;
        }


        void IEnvironment.OpenURL(string url) => Application.OpenURL(url);


        void IEnvironment.StartGameplay()
        {
            if (_gameplayStarted) return;

            _gameplayStarted = true;
            GP_Game.GameplayStart();
        }

        void IEnvironment.StopGameplay()
        {
            if (!_gameplayStarted) return;

            _gameplayStarted = false;
            GP_Game.GameplayStop();
        }

        bool IEnvironment.ChannelIsMuted(Sound.Channel channel)
            => channel == Sound.Channel.Music ? !Sound.Settings.MusicEnabled : !Sound.Settings.SfxEnabled;

        void IEnvironment.SetChannelMute(Sound.Channel channel, bool isMute)
        {
            if (channel == Sound.Channel.Music)
            {
                if (isMute) GP_Sounds.Mute(SoundType.Music);
                else GP_Sounds.Unmute(SoundType.Music);

                Sound.Settings.MusicEnabled = !isMute;
            }

            if (channel == Sound.Channel.SFX)
            {
                if (isMute) GP_Sounds.Mute(SoundType.SFX);
                else GP_Sounds.Unmute(SoundType.SFX);

                Sound.Settings.SfxEnabled = !isMute;
            }

            IEnvironment.InvokeChangeSoundChannel();
        }

        async void IEnvironment.ToggleFullscreen()
        {
            bool isFullscreenPrevious = GP_Fullscreen.IsEnabled();
            GP_Fullscreen.Toggle();

            await UniTask.WaitUntil(() => isFullscreenPrevious != GP_Fullscreen.IsEnabled());

            IEnvironment.InvokeChangeFullscreen();
        }

        void IEnvironment.Invite() => GP_Socials.Invite();
#else
        public static bool IsAvailableForSelection => false;

        Language IEnvironment.DeviceLanguage => default;

        DeviceType IEnvironment.DeviceType => default;

        bool IInitializable.Initialized => false;

        bool ISelectableService.IsAvailableForSelection => false;

        bool IEnvironment.FullscreenIsSupported => false;

        bool IEnvironment.IsFullscreen => false;

        bool IEnvironment.InviteAvailable => false;

        void IEnvironment.GameReady() { }

        void IInitializable.Initialize() { }

        void IEnvironment.OpenURL(string url) { }

        void IEnvironment.StartGameplay() { }

        void IEnvironment.StopGameplay() { }

        bool IEnvironment.ChannelIsMuted(Sound.Channel channel) => false;

        void IEnvironment.SetChannelMute(Sound.Channel channel, bool isMute) { }

        void IEnvironment.ToggleFullscreen() { }

        void IEnvironment.Invite() { }
#endif
    }
}
