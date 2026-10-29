using UnityEngine;


namespace Nodes.Providers
{
    public abstract class ProviderConfiguration : MonoBehaviour
    {
        public abstract EnvKey EnvKey { get; }
        public abstract Env.IProvider EnvProvider { get; }
        public abstract Ads.IProvider AdsProvider { get; }
        public abstract Socials.IProvider SocialsProvider { get; }
        public abstract Payments.IProvider PaymentsProvider { get; }
        public abstract GameReady.IProvider GameReadyProvider { get; }
        public abstract CloudSaves.IProvider CloudSavesProvider { get; }

    }
}

