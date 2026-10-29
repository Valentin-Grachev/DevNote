using System;
using UnityEngine;

namespace Nodes.Providers.Test
{
    public class TestEnvProvider : MonoBehaviour, Env.IProvider
    {
        private enum DeviceType { Desktop, Mobile }


        [SerializeField] private string _platformName = "Test";
        [SerializeField] private DeviceType _deviceType;
        [SerializeField] private SystemLanguage _language = SystemLanguage.English;

        EnvKey Env.IProvider.EnvKey => EnvKey.Test;
        string Env.IProvider.PlatformName => _platformName;
        bool Env.IProvider.IsMobile => _deviceType == DeviceType.Mobile;
        bool Env.IProvider.IsDesktop => _deviceType == DeviceType.Desktop;
        SystemLanguage Env.IProvider.Language => _language;

        void ICommonProvider.Initialize(Action onInitialized) => onInitialized?.Invoke();
    }
}

