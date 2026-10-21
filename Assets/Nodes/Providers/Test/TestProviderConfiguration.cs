using UnityEngine;

namespace Nodes.Providers.Test
{
    public class TestProviderConfiguration : ProviderConfiguration
    {
        [SerializeField] private TestAdsProvider _adsProvider;
        [SerializeField] private TestSocialsProvider _socialsProvider;



        public override Ads.IProvider AdsProvider => _adsProvider;
        public override Socials.IProvider SocialsProvider => _socialsProvider;
        public override Payments.IProvider PaymentsProvider => throw new System.NotImplementedException();


    }
}


