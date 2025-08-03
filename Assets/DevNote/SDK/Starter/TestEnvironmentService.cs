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

        bool IProjectInitializable.Initialized => _initialized;

        async void IProjectInitializable.Initialize() 
        { 
            await UniTask.WaitForSeconds(_delayBeforeInitialization);
            _initialized = true;
        }

        bool ISelectableService.Available => true;

        Language IEnvironment.CurrentLanguage => _currentLanguage;

        DeviceType IEnvironment.DeviceType => _deviceType;

        DateTime IEnvironment.ServerTime => DateTime.Now;

        void IEnvironment.GameReady() => Debug.Log($"{Info.Prefix} Game ready");

        
    }
}


