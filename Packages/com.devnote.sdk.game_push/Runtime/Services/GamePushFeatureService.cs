#if UNITY_WEBGL
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GamePush;
#endif
using UnityEngine;

namespace DevNote.SDK.GamePush
{
    public class GamePushFeatureService : MonoBehaviour, IFeature
    {
#if UNITY_WEBGL
        private bool _initialized = false; bool IInitializable.Initialized => _initialized;

        private readonly List<Platform> START_MENU_PLATFORMS = new()
        {
            Platform.OK,
            Platform.VK,
        };

        bool IFeature.IsStartMenu => START_MENU_PLATFORMS.Contains(GP_Platform.Type());


        bool ISelectableService.IsAvailableForSelection =>
            GamePushEnvironmentService.IsAvailableForSelection;


        async void IInitializable.Initialize()
        {
            await UniTask.WaitUntil(() => GP_Init.isReady);
            _initialized = true;
        }
#else
        bool IFeature.IsStartMenu => false;

        bool IInitializable.Initialized => false;

        bool ISelectableService.IsAvailableForSelection => false;

        void IInitializable.Initialize() { }
#endif
    }
}
