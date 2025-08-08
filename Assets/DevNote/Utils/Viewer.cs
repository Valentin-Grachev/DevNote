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

        public Viewer(T prefab, ViewerMode mode)
        {
            _mode = mode;
            _prefab = prefab;
            _viewInstance = null;
        }

        public Viewer(T instance)
        {
            _mode = ViewerMode.EnableDisable;
            _prefab = null;
            _viewInstance = instance;
        }



        public T Show(RectTransform container)
        {
            if (_viewInstance == null)
                _viewInstance = UnityEngine.Object.Instantiate(_prefab, container);
            
            else _viewInstance.gameObject.SetActive(true);

            var rectTransform = _viewInstance.transform as RectTransform;
            rectTransform.SetParent(container, false);

            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.SetAsLastSibling();

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



