using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GamePush;
using GamePush.Initialization;
using UnityEngine;


namespace DevNote.SDK.GamePush
{
    public class GamePushEnvironmentService : MonoBehaviour, IEnvironment
    {
        private bool _initialized = false;
        private bool _gameplayStarted = false;


        private readonly List<Platform> DISTRIBUTIONS_SUPPORTS_FULLSCREEN = new()
        {
            Platform.OK,
            Platform.VK, 
        };



        public static bool IsAvailableForSelection 
            => IEnvironment.EnvironmentKey == EnvironmentKey.GamePush && !IEnvironment.IsEditor;

        Language IEnvironment.DeviceLanguage => GP_Language.Current() switch
        {
            global::GamePush.Language.English => Language.EN,
            global::GamePush.Language.Russian => Language.RU,

            _ => Language.EN,
        };

        DeviceType IEnvironment.DeviceType => GP_Device.IsMobile() ? DeviceType.Mobile : DeviceType.Desktop;

        bool IInitializable.Initialized => _initialized;

        bool ISelectableService.IsAvailableForSelection => IsAvailableForSelection;

        bool IEnvironment.FullscreenIsSupported => 
            DISTRIBUTIONS_SUPPORTS_FULLSCREEN.Contains(GP_Platform.Type());

        bool IEnvironment.IsFullscreen => GP_Fullscreen.IsEnabled();

        void IEnvironment.GameReady() => GP_Game.GameReady();


        async void IInitializable.Initialize()
        {
            GP_Initialization.Execute();

            await UniTask.WaitUntil(() => GP_Init.isReady && Sound.Initialized);

            IEnvironment.StartGameUtcTime = GP_Server.Time();

            Sound.Settings.MusicEnabled = !GP_Sounds.IsMuted(SoundType.Music);
            Sound.Settings.SfxEnabled = !GP_Sounds.IsMuted(SoundType.SFX);

            GP_Sounds.OnMuteMusic += () =>
            {
                Sound.Settings.MusicEnabled = false;
                IEnvironment.InvokeChangeSoundChannel();
            };
            GP_Sounds.OnMuteSFX += () =>
            {
                Sound.Settings.SfxEnabled = false;
                IEnvironment.InvokeChangeSoundChannel();
            };
            GP_Sounds.OnUnmuteMusic += () =>
            {
                Sound.Settings.MusicEnabled = true;
                IEnvironment.InvokeChangeSoundChannel();
            };
            GP_Sounds.OnUnmuteSFX += () =>
            {
                Sound.Settings.SfxEnabled = true;
                IEnvironment.InvokeChangeSoundChannel();
            };

            GP_Fullscreen.OnFullscreenChange += () => IEnvironment.InvokeChangeFullscreen();

            _initialized = true;
        }


        void IEnvironment.OpenURL(string url) 
            => Debug.LogError($"[{nameof(GamePushEnvironmentService)}] Open URL is not supported");


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
            => channel == Sound.Channel.Music ? GP_Sounds.IsMuted(SoundType.Music) : GP_Sounds.IsMuted(SoundType.SFX);

        void IEnvironment.SetChannelMute(Sound.Channel channel, bool isMute)
        {
            if (channel == Sound.Channel.Music)
            {
                if (isMute) GP_Sounds.Mute(SoundType.Music);
                else GP_Sounds.Unmute(SoundType.Music);
            }

            if (channel == Sound.Channel.SFX)
            {
                if (isMute) GP_Sounds.Mute(SoundType.SFX);
                else GP_Sounds.Unmute(SoundType.SFX);
            }
                
        }

        async void IEnvironment.ToggleFullscreen()
        {
            bool isFullscreenPrevious = GP_Fullscreen.IsEnabled();
            GP_Fullscreen.Toggle();

            await UniTask.WaitUntil(() => isFullscreenPrevious != GP_Fullscreen.IsEnabled());

            IEnvironment.InvokeChangeFullscreen();
        }
    }
}


