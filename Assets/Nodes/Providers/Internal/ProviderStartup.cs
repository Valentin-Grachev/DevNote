using System;
using System.Collections.Generic;
using UnityEngine;

namespace Nodes.Providers
{
    public class ProviderStartup : MonoBehaviour
    {
        [SerializeField] private EnvKey _editorConfigurationKey;
        [SerializeField] private EnvKey _buildConfigurationKey;
        [SerializeField] private List<ProviderConfiguration> _configurations;

        public void InitializeProviders(Action onInitialized)
        {
#if UNITY_EDITOR
            var configurationKey = _editorConfigurationKey;
#else
            var configurationKey = _buildConfigurationKey;
#endif

            var configuration = _configurations.Find
                (configuration => configuration.EnvKey == configurationKey);

            int initProcessCount = 6;

            Env.InitializeProvider(configuration.EnvProvider, OnProviderInitialized);
            Ads.InitializeProvider(configuration.AdsProvider, OnProviderInitialized);
            Payments.InitializeProvider(configuration.PaymentsProvider, OnProviderInitialized);
            Socials.InitializeProvider(configuration.SocialsProvider, OnProviderInitialized);
            GameReady.InitializeProvider(configuration.GameReadyProvider, OnProviderInitialized);
            CloudSaves.InitializeProvider(configuration.CloudSavesProvider, OnProviderInitialized);


            void OnProviderInitialized()
            {
                initProcessCount--;
                if (initProcessCount == 0) onInitialized?.Invoke();
            }

        }

    }
}


