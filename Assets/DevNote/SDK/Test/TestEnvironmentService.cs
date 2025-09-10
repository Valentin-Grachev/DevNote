using System;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace DevNote.Services.Test
{
    public class TestEnvironmentService : MonoBehaviour, IEnvironment
    {
        [SerializeField] private float _delayBeforeInitialization;
        [SerializeField] private Language _currentLanguage = Language.EN;
        [SerializeField] private DeviceType _deviceType = DeviceType.Desktop;


        private bool _initialized = false;

        bool IInitializable.Initialized => _initialized;

        async void IInitializable.Initialize() 
        { 
            await UniTask.WaitForSeconds(_delayBeforeInitialization);
            IEnvironment.StartGameTime = DateTime.Now;
            _initialized = true;
        }

        bool ISelectableService.Available => true;

        Language IEnvironment.CurrentLanguage => _currentLanguage;

        DeviceType IEnvironment.DeviceType => _deviceType;

        void IEnvironment.GameReady() => Debug.Log($"{Info.Prefix} Game ready");

    }
}


