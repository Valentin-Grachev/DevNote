using System;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace DevNote.SDK.Test
{
    public class TestEnvironmentService : MonoBehaviour, IEnvironment
    {
        [SerializeField] private float _delayBeforeInitialization;
        [SerializeField] private Language _deviceLanguage = Language.EN;
        [SerializeField] private DeviceType _deviceType = DeviceType.Desktop;


        private bool _initialized = false;
        private bool _gameplayStarted = false;

        bool IInitializable.Initialized => _initialized;

        async void IInitializable.Initialize() 
        { 
            await UniTask.WaitForSeconds(_delayBeforeInitialization);
            IEnvironment.StartGameUtcTime = DateTime.Now;

            _initialized = true;
        }

        bool ISelectableService.IsAvailableForSelection => true;

        Language IEnvironment.DeviceLanguage => _deviceLanguage;

        DeviceType IEnvironment.DeviceType => _deviceType;

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
    }
}


