using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    [System.Serializable]
    public class ProductIdConvertor
    {
        [System.Serializable]
        private struct ProductTypeId
        {
            public string key;
            public string id;
        }

        [SerializeField] private List<ProductTypeId> _productConvertors;

        public string GetProductId(string productKey)
            => _productConvertors.Find((typeId) => typeId.key == productKey).id;

        public string GetProductKey(string productId)
            => _productConvertors.Find((typeId) => typeId.id == productId).key;



    }
}


