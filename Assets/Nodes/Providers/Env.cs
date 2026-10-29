using System;
using UnityEngine;

namespace Nodes.Providers
{
    public static class Env
    {
        public interface IProvider : ICommonProvider
        {
            public EnvKey EnvKey { get; }
            public string PlatformName { get; }
            public bool IsMobile { get; }
            public bool IsDesktop { get; }
            public SystemLanguage Language { get; }
        }

        private static IProvider _provider;

        public static void InitializeProvider(IProvider provider, Action onInitialized)
            => provider.Initialize(() =>
            {
                _provider = provider;
                onInitialized?.Invoke();
            });

        public static bool IsInitialized => _provider != null;


        public static bool IsEditor
        {
            get
            {
#if UNITY_EDITOR
                return true;
#else
                return false;
#endif
            }
        }

        public static EnvKey EnvKey => _provider.EnvKey;
        public static string PlatformName => _provider.PlatformName;
        public static bool IsMobile => _provider.IsMobile;
        public static bool IsDesktop => _provider.IsDesktop;
        public static SystemLanguage Language => _provider.Language;

    }
}

