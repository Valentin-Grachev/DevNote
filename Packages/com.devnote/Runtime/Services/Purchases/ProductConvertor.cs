using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    [Serializable] public class ProductConverter
    {
        [Serializable] private struct ConvertData
        {
            public ProductKey productKey;
            public string id;
        }

        [SerializeField] private List<ConvertData> _convertableProducts;

        public string GetProductId(ProductKey productKey)
        {
            if (_convertableProducts.TryFind((data) => data.productKey == productKey, out ConvertData convertData))
                return convertData.id;

            return productKey.ToString();
        }


        public ProductKey GetProductKey(string productId)
        {
            if (_convertableProducts.TryFind((data) => data.id == productId, out ConvertData convertData))
                return convertData.productKey;

            return productId.ToEnum<ProductKey>();
        }



    }
}


