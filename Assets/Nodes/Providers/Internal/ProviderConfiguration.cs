using UnityEngine;


namespace Nodes.Providers
{
    public abstract class ProviderConfiguration : MonoBehaviour
    {
        public abstract Ads.IProvider AdsProvider { get; }
        public abstract Socials.IProvider SocialsProvider { get; }
        public abstract Payments.IProvider PaymentsProvider { get; }


    }
}

