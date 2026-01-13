using System;
using UnityEngine;


namespace DevNote
{

    public class Viewer<T> where T : Component
    {
        public event Action OnShown;
        public event Action OnHidden;

        private T _prefab;
        private T _viewInstance; public T View => _viewInstance;

        public bool ViewExists => _viewInstance != null;

        public Viewer(T view)
        {
            _prefab = view;

            bool isPrefab = view.gameObject.scene == null || view.gameObject.scene.IsValid() == false;
            _viewInstance = isPrefab ? null : view;
        }

        public T Show(Transform container = null)
        {
            bool wasShownBefore = _viewInstance != null && _viewInstance.gameObject.activeSelf;

            if (_viewInstance == null)
                _viewInstance = UnityEngine.Object.Instantiate(_prefab, container);

            else _viewInstance.gameObject.SetActive(true);

            _viewInstance.transform.SetAsLastSibling();

            if (!wasShownBefore) OnShown?.Invoke();

            return _viewInstance;
        }

        public T ShowExpand(RectTransform container)
        {
            bool wasShown = _viewInstance != null && _viewInstance.gameObject.activeSelf;

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

            if (!wasShown) OnShown?.Invoke();

            return _viewInstance;
        }


        public void Hide()
        {
            if (_viewInstance == null || _viewInstance.gameObject.activeSelf == false)
                return;

            _viewInstance.gameObject.SetActive(false);

            OnHidden?.Invoke();
        }



    }
}


