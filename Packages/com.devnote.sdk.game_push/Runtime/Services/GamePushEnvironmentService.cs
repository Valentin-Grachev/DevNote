using Cysharp.Threading.Tasks;
using GamePush;
using UnityEngine;


namespace DevNote.SDK.GamePush
{
    public class GamePushEnvironmentService : MonoBehaviour, IEnvironment
    {
        private bool _initialized = false;


        public static bool IsAvailableForSelection => IEnvironment.EnvironmentKey == EnvironmentKey.GamePush;

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
            await UniTask.WaitUntil(() => GP_Init.isReady);

            IEnvironment.StartGameUtcTime = GP_Server.Time();

            _initialized = true;
        }

        void IEnvironment.OpenURL(string url) 
            => Debug.LogError($"[{nameof(GamePushEnvironmentService)}] Open URL is not supported");
    }
}


