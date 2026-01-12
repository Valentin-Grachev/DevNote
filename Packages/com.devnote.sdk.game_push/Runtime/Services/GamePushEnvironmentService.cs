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


        public static bool IsAvailableForSelection 
            => IEnvironment.EnvironmentKey == EnvironmentKey.GamePush;

        Language IEnvironment.DeviceLanguage => GP_Language.Current() switch
        {
            global::GamePush.Language.English => Language.EN,
            global::GamePush.Language.Russian => Language.RU,

            _ => Language.EN,
        };

        DeviceType IEnvironment.DeviceType => GP_Device.IsMobile() ? DeviceType.Mobile : DeviceType.Desktop;

        bool IInitializable.Initialized => _initialized;

        bool ISelectableService.IsAvailableForSelection => IsAvailableForSelection;

        void IEnvironment.GameReady() => GP_Game.GameReady();


        async void IInitializable.Initialize()
        {
            GP_Initialization.Execute();

            await UniTask.WaitUntil(() => GP_Init.isReady && Sound.Initialized);

            IEnvironment.StartGameUtcTime = GP_Server.Time();

            Sound.Settings.MusicEnabled = !GP_Sounds.IsMuted(SoundType.Music);
            Sound.Settings.SfxEnabled = !GP_Sounds.IsMuted(SoundType.SFX);

            GP_Sounds.OnMuteMusic += () => Sound.Settings.MusicEnabled = false;
            GP_Sounds.OnMuteSFX += () => Sound.Settings.SfxEnabled = false;
            GP_Sounds.OnUnmuteMusic += () => Sound.Settings.MusicEnabled = true;
            GP_Sounds.OnUnmuteSFX += () => Sound.Settings.SfxEnabled = true;

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

        void IEnvironment.SetChannelMute(Sound.Channel channel, bool value)
        {
            if (channel == Sound.Channel.Music)
            {
                if (value) GP_Sounds.Mute(SoundType.Music);
                else GP_Sounds.Unmute(SoundType.Music);
            }

            if (channel == Sound.Channel.SFX)
            {
                if (value) GP_Sounds.Mute(SoundType.SFX);
                else GP_Sounds.Unmute(SoundType.SFX);
            }
                
        }


    }
}


