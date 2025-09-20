using System.Collections.Generic;

namespace DevNote
{
    public interface IPurchaseHandler
    {
        private static IPurchaseHandler _handler;
        public static void SetHandler(IPurchaseHandler handler) => _handler = handler;

        public static void HandlePurchaseStatic(string productKey) => _handler.HandlePurchase(productKey);
        public static bool ProductIsConsumable(string productKey) => _handler.ConsumableProductKeys.Contains(productKey);


        protected abstract void HandlePurchase(string productKey);
        protected abstract List<string> ConsumableProductKeys { get; }

    }
}


