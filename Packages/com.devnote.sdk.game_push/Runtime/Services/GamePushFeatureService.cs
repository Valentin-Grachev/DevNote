using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GamePush;
using UnityEngine;

namespace DevNote.SDK.GamePush
{
    public class GamePushFeatureService : MonoBehaviour, IFeature
    {
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
    }
}



