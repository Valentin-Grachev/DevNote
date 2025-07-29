using System;
using UnityEngine;


namespace DevNote
{
    public enum ViewerMode { InstantiateDestroy, EnableDisable }

    public class Viewer<T> where T : Component
    {
        public event Action OnShown;
        public event Action OnHidden;

        private ViewerMode _mode;
        private T _prefab;
        private T _viewInstance; public T View => _viewInstance;
        private RectTransform _container;

        public Viewer(T prefab, RectTransform container, ViewerMode mode)
        {
            _mode = mode;
            _prefab = prefab;
            _container = container;
            _viewInstance = null;
        }

        public Viewer(T instance)
        {
            _mode = ViewerMode.EnableDisable;
            _prefab = null;
            _container = null;
            _viewInstance = instance;
        }



        public T Show()
        {
            if (_viewInstance == null)
                _viewInstance = UnityEngine.Object.Instantiate(_prefab, _container);
            
            else _viewInstance.gameObject.SetActive(true);

            OnShown?.Invoke();
            return _viewInstance;
        }


        public void Hide()
        {
            switch (_mode)
            {
                case ViewerMode.InstantiateDestroy:
                    UnityEngine.Object.Destroy(_viewInstance.gameObject);
                    _viewInstance = null;
                    break;

                case ViewerMode.EnableDisable:
                    _viewInstance.gameObject.SetActive(false);
                    break;
            }

            OnHidden?.Invoke();
        }



    }
}



