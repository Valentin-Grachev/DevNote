using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    public class Pool<T> where T : Component
    {
        private T _prefab;
        private List<T> _poolObjects;


        public Pool(T prefab)
        {
            _prefab = prefab;
            _poolObjects = new();
        }

        public T Get()
        {
            var poolObject = _poolObjects.Find(poolObject => !poolObject.gameObject.activeSelf);

            if (poolObject != null)
                poolObject.gameObject.SetActive(true);

            else
            {
                poolObject = Object.Instantiate(_prefab);
                _poolObjects.Add(poolObject);
            }

            return poolObject;
        }

        public void Clear()
        {
            for (int i = 0; i < _poolObjects.Count; i++)
                ReturnToPool(_poolObjects[i]);
        }

        public void ReturnToPool(T poolObject) => poolObject.gameObject.SetActive(false);



    }
}


