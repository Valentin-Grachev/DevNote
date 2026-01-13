using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    public class Pool<T> where T : Component
    {
        private T _prefab;
        private List<T> _poolObjects;
        private Transform _container;


        public Pool(T poolObject, Transform container = null)
        {
            _prefab = poolObject;
            _poolObjects = new();
            _container = container;

            if (!poolObject.gameObject.IsPrefab())
            {
                poolObject.gameObject.SetActive(false);
                _poolObjects.Add(poolObject);
            }
        }

        public void Expand(int amount)
        {
            for (int i = _poolObjects.Count; i < amount; i++)
            {
                var poolObject = Object.Instantiate(_prefab, _container);
                poolObject.gameObject.SetActive(false);
                _poolObjects.Add(poolObject);
            }
        }


        public T Get(Transform container = null)
        {
            if (container == null) container = _container;

            var poolObject = _poolObjects.Find(poolObject => !poolObject.gameObject.activeSelf);

            if (poolObject != null)
                poolObject.gameObject.SetActive(true);

            else
            {
                poolObject = Object.Instantiate(_prefab, container);
                _poolObjects.Add(poolObject);
            }

            if (poolObject.transform.parent != container)
                poolObject.transform.SetParent(container);

            return poolObject;
        }

        public void Clear()
        {
            for (int i = 0; i < _poolObjects.Count; i++)
                Return(_poolObjects[i]);
        }

        public void Return(T poolObject) => poolObject.gameObject.SetActive(false);



    }
}



