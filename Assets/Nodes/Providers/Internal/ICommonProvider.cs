using System;

namespace Nodes.Providers
{
    public interface ICommonProvider
    {
        public void Initialize(Action onInitialized);
    }
}


