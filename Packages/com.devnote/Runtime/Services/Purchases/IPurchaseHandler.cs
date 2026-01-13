using System.Collections.Generic;

namespace DevNote
{
    public interface IPurchaseHandler
    {
        private static IPurchaseHandler _handler;
        public static void SetHandler(IPurchaseHandler handler) => _handler = handler;

        public static void HandlePurchaseStatic(ProductKey productKey) => _handler.HandlePurchase(productKey);
        public static bool ProductIsConsumable(ProductKey productKey) => _handler.ConsumableProductKeys.Contains(productKey);


        protected abstract void HandlePurchase(ProductKey productKey);
        protected abstract List<ProductKey> ConsumableProductKeys { get; }

    }
}


