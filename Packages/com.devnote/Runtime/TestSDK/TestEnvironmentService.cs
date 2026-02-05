using System;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace DevNote.SDK.Test
{
    public class TestEnvironmentService : MonoBehaviour, IEnvironment
    {
        [SerializeField] private DistributionKey _distributionKey;
        [SerializeField] private bool _fullscreenIsSupported;
        [SerializeField] private Language _deviceLanguage = Language.EN;
        [SerializeField] private DeviceType _deviceType = DeviceType.Desktop;

        private bool _isFullscreen = false;
        private bool _initialized = false;
        private bool _gameplayStarted = false;

        bool IInitializable.Initialized => _initialized;

        void IInitializable.Initialize() 
        { 
            IEnvironment.StartGameUtcTime = DateTime.Now;
            IEnvironment.DistributionKey = _distributionKey;

            _initialized = true;
        }

        bool ISelectableService.IsAvailableForSelection => true;

        Language IEnvironment.DeviceLanguage => _deviceLanguage;

        DeviceType IEnvironment.DeviceType => _deviceType;

        bool IEnvironment.FullscreenIsSupported => _fullscreenIsSupported;

        bool IEnvironment.IsFullscreen => _isFullscreen;

        void IEnvironment.GameReady() => Debug.Log($"{Info.Prefix} Game ready");

        void IEnvironment.OpenURL(string url) => Application.OpenURL(url);

        void IEnvironment.StartGameplay()
        {
            if (_gameplayStarted) return;

            _gameplayStarted = true;
            Debug.Log($"{Info.Prefix} Start gameplay");
        }

        void IEnvironment.StopGameplay()
        {
            if (!_gameplayStarted) return;

            _gameplayStarted = false;
            Debug.Log($"{Info.Prefix} Stop gameplay");
        }

        void IEnvironment.SetChannelMute(Sound.Channel channel, bool value)
        {
            if (channel == Sound.Channel.Music)
                Sound.Settings.MusicEnabled = !value;

            if (channel == Sound.Channel.SFX)
                Sound.Settings.SfxEnabled = !value;
        }

        bool IEnvironment.ChannelIsMuted(Sound.Channel channel) 
            => channel == Sound.Channel.Music ? !Sound.Settings.MusicEnabled : !Sound.Settings.SfxEnabled;

        void IEnvironment.SetFullscreen(bool active)
        {
            _isFullscreen = active;
            Debug.Log($"{Info.Prefix} Set fullscreen: {active}");
        }
    }
}


