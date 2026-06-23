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
        private bool _musicEnabledByUser = true;
        private bool _sfxEnabledByUser = true;


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

        bool IEnvironment.InviteAvailable => GP_Socials.IsSupportsNativeInvite();

        void IEnvironment.GameReady() => GP_Game.GameReady();


        async void IInitializable.Initialize()
        {
            GP_Initialization.Execute();

            await UniTask.WaitUntil(() => GP_Init.isReady && Sound.Initialized);

            IEnvironment.StartGameUtcTime = GP_Server.Time();
            IEnvironment.GameStoreName = GP_Platform.Type().ToString().ToLower();

            _musicEnabledByUser = Sound.Settings.MusicEnabled;
            _sfxEnabledByUser = Sound.Settings.SfxEnabled;

            UpdateMusicState(GP_Sounds.IsMuted(SoundType.Music) ? false : _musicEnabledByUser);
            UpdateSfxState(GP_Sounds.IsMuted(SoundType.SFX) ? false : _sfxEnabledByUser);

            GP_Sounds.OnMuteMusic += OnMuteMusic;
            GP_Sounds.OnMuteSFX += OnMuteSfx;
            GP_Sounds.OnUnmuteMusic += OnUnmuteMusic;
            GP_Sounds.OnUnmuteSFX += OnUnmuteSfx;

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
            => channel == Sound.Channel.Music ? !Sound.Settings.MusicEnabled : !Sound.Settings.SfxEnabled;

        void IEnvironment.SetChannelMute(Sound.Channel channel, bool isMute)
        {
            if (channel == Sound.Channel.Music)
            {
                _musicEnabledByUser = !isMute;
                UpdateMusicState(_musicEnabledByUser);

                if (isMute) GP_Sounds.Mute(SoundType.Music);
                else GP_Sounds.Unmute(SoundType.Music);
            }

            if (channel == Sound.Channel.SFX)
            {
                _sfxEnabledByUser = !isMute;
                UpdateSfxState(_sfxEnabledByUser);

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

        void IEnvironment.Invite() => GP_Socials.Invite();

        private void OnMuteMusic() => UpdateMusicState(false);

        private void OnMuteSfx() => UpdateSfxState(false);

        private void OnUnmuteMusic() => UpdateMusicState(_musicEnabledByUser);

        private void OnUnmuteSfx() => UpdateSfxState(_sfxEnabledByUser);

        private void UpdateMusicState(bool enabled)
        {
            if (Sound.Settings.MusicEnabled == enabled)
                return;

            Sound.Settings.MusicEnabled = enabled;
            IEnvironment.InvokeChangeSoundChannel();
        }

        private void UpdateSfxState(bool enabled)
        {
            if (Sound.Settings.SfxEnabled == enabled)
                return;

            Sound.Settings.SfxEnabled = enabled;
            IEnvironment.InvokeChangeSoundChannel();
        }

    }
}


