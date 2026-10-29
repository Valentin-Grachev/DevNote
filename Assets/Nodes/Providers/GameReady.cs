using System;


namespace Nodes.Providers
{
    public static class GameReady
    {

        public interface IProvider : ICommonProvider
        {
            public void GameplayReady();
            public void GameplayStart();
            public void GameplayPause();

        }


        private static IProvider _provider;

        public static void InitializeProvider(IProvider provider, Action onInitialized)
            => provider.Initialize(() =>
            {
                _provider = provider;
                onInitialized?.Invoke();
            });

        public static bool IsInitialized => _provider != null;

        public static void GameplayReady() => _provider.GameplayReady();
        public static void GameplayStart() => _provider.GameplayStart();
        public static void GameplayPause() => _provider.GameplayPause();


    }

}


