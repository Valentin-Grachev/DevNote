using System;
using UnityEngine;

namespace Nodes.Providers
{
    public class ProviderStartup : MonoBehaviour
    {
        [SerializeField] private ProviderConfiguration _editorConfiguration;
        [SerializeField] private ProviderConfiguration _buildConfiguration;

        public void InitializeProviders(Action onInitialized)
        {
#if UNITY_EDITOR
            var configuration = _editorConfiguration;
#else
            var configuration = _buildConfiguration;
#endif

            int initProcessCount = 2;

            Ads.InitializeProvider(configuration.AdsProvider, OnProviderInitialized);
            Payments.InitializeProvider(configuration.PaymentsProvider, OnProviderInitialized);
            Socials.InitializeProvider(configuration.SocialsProvider, OnProviderInitialized);


            void OnProviderInitialized()
            {
                initProcessCount--;
                if (initProcessCount == 0) onInitialized?.Invoke();
            }

        }

    }
}


