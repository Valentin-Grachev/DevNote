using UnityEngine;

namespace Nodes.Providers.Test
{
    public class TestProviderConfiguration : ProviderConfiguration
    {
        [SerializeField] private TestEnvProvider _envProvider;
        [SerializeField] private TestAdsProvider _adsProvider;
        [SerializeField] private TestPaymentsProvider _paymentsProvider;
        [SerializeField] private TestSocialsProvider _socialsProvider;
        [SerializeField] private TestGameReadyProvider _gameReadyProvider;
        [SerializeField] private TestCloudSavesProvider _cloudSaveProvider;


        public override EnvKey EnvKey => EnvKey.Test;
        public override Env.IProvider EnvProvider => _envProvider;
        public override Ads.IProvider AdsProvider => _adsProvider;
        public override Socials.IProvider SocialsProvider => _socialsProvider;
        public override Payments.IProvider PaymentsProvider => _paymentsProvider;
        public override GameReady.IProvider GameReadyProvider => _gameReadyProvider;
        public override CloudSaves.IProvider CloudSavesProvider => _cloudSaveProvider;
    }
}


