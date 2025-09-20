using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    [Serializable] public class ProductConvertor
    {
        [Serializable] private struct ConvertorData
        {
            public string key;
            public string id;
        }

        [SerializeField] private List<ConvertorData> _convertions;

        public string GetProductId(string productKey)
            => _convertions.Find((typeId) => typeId.key == productKey).id;

        public string GetProductKey(string productId)
            => _convertions.Find((typeId) => typeId.id == productId).key;



    }
}


