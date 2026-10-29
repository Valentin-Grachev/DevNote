using System;

namespace Nodes.Providers
{
    public static class CloudSaves
    {
        public interface IProvider : ICommonProvider
        {
            public bool TryGetValue(string key, out string value);
            public void SetValue(string key, string value);
            public void Commit(Action<bool> onHandled = null);

        }

        private static IProvider _provider;

        public static void InitializeProvider(IProvider provider, Action onInitialized)
            => provider.Initialize(() =>
            {
                _provider = provider;
                onInitialized?.Invoke();
            });

        public static bool IsInitialized => _provider != null;


        public static string GetValue(string key, string defaultValue = "")
        {
            bool success = _provider.TryGetValue(key, out var value);
            return success ? value : defaultValue;
        }
        public static void SetValue(string key, string value) 
            => _provider.SetValue(key, value);
        public static void Commit(Action<bool> onHandled = null) 
            => _provider.Commit(onHandled);



    }

}
