
using System;

namespace Nodes.Providers
{

    public static class Socials
    {
        public interface IProvider : ICommonProvider
        {
            public bool IsSupportInvite { get; }
            public bool IsSupportRateGame { get; }
            public bool IsSupportOpenUrl { get; }

            public void Invite();
            public void RateGame();
            public void OpenUrl(string url);
        }

        private static IProvider _provider;

        public static void InitializeProvider(IProvider provider, Action onInitialized)
            => provider.Initialize(() =>
            {
                _provider = provider;
                onInitialized?.Invoke();
            });

        public static bool IsInitialized => _provider != null;
        public static bool IsSupportInvite => _provider.IsSupportInvite;
        public static bool IsSupportRateGame => _provider.IsSupportRateGame;

        public static void Invite() => _provider.Invite();
        public static void RateGame() => _provider.RateGame();
        public static void OpenUrl(string url) => _provider.OpenUrl(url);


    }
}



