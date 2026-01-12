using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace DevNote
{
    public class OrientationAdapter : MonoBehaviour
    {
        public enum ChangeOption { Scale, Container, LocalPosition, Active }

        [SerializeField, Expandable] private OrientationAdapterConfig _config;


        [BoxGroup("Active:"), ShowIf(nameof(ShowChangeActive)), SerializeField, Label("Is Active In Portrait")] private bool _portraitGameObjectIsActive = true;
        [BoxGroup("Active:"), ShowIf(nameof(ShowChangeActive)), SerializeField, Label("Is Active In Landscape")] private bool _landscapeGameObjectIsActive = true;

        [BoxGroup("Container:"), ShowIf(nameof(ShowChangeContainer)), SerializeField, Label("Portrait Container")] private RectTransform _portraitContainer;
        [BoxGroup("Container:"), ShowIf(nameof(ShowChangeContainer)), SerializeField, Label("Landscape Container")] private RectTransform _landscapeContainer;

        [BoxGroup("Local Position:"), ShowIf(nameof(ShowChangeLocalPosition)), SerializeField, Label("Portrait Position")] private Vector2 _portraitLocalPosition;
        [BoxGroup("Local Position:"), ShowIf(nameof(ShowChangeLocalPosition)), SerializeField, Label("Landscape Position")] private Vector2 _landscapeLocalPosition;

        [BoxGroup("Scale:"), ShowIf(nameof(ShowChangeScale)), SerializeField, Label("Portrait Scale")] private float _portraitScale;
        [BoxGroup("Scale:"), ShowIf(nameof(ShowChangeScale)), SerializeField, Label("Landscape Scale")] private float _landscapeScale;

        [BoxGroup(""), SerializeField, ShowIf(nameof(ShowChangeOptions))] private List<ChangeOption> _changeOptions = new();

        private bool UseConfig => _config != null;
        private bool ShowChangeScale => _config == null && _changeOptions.Contains(ChangeOption.Scale);
        private bool ShowChangeContainer => _config == null && _changeOptions.Contains(ChangeOption.Container);
        private bool ShowChangeLocalPosition => _config == null && _changeOptions.Contains(ChangeOption.LocalPosition);
        private bool ShowChangeActive => _config == null && _changeOptions.Contains(ChangeOption.Active);
        private bool ShowChangeOptions => _config == null;




        private void Start()
        {
            ScreenState.OnOrientationChanged += OnScreenOrientationChanged;
            ApplyOrientation(ScreenState.Orientation);
        }

        private void OnDestroy()
        {
            ScreenState.OnOrientationChanged -= OnScreenOrientationChanged;
        }

        private void OnScreenOrientationChanged() => ApplyOrientation(ScreenState.Orientation);


        private void ApplyOrientation(Orientation orientation)
        {
            bool isPortrait = orientation == Orientation.Portrait;

            // Scale
            bool isChangeScale = UseConfig ? _config.IsChangeScale : _changeOptions.Contains(ChangeOption.Scale);
            float portraitScale = UseConfig ? _config.PortraitScale : _portraitScale;
            float landscapeScale = UseConfig ? _config.LandscapeScale : _landscapeScale;
            UpdateScale(isChangeScale, isPortrait, portraitScale, landscapeScale);

            // Local position
            bool isChangePosition = UseConfig ? _config.IsChangeLocalPosition : _changeOptions.Contains(ChangeOption.LocalPosition);
            Vector2 portraitPosition = UseConfig ? _config.PortraitLocalPosition : _portraitLocalPosition;
            Vector2 landscapePosition = UseConfig ? _config.LandscapeLocalPosition : _landscapeLocalPosition;
            UpdateLocalPosition(isChangePosition, isPortrait, portraitPosition, landscapePosition);

            // Container
            UpdateContainer(_changeOptions.Contains(ChangeOption.Container) && _config == null, 
                isPortrait, _portraitContainer, _landscapeContainer);

            // Active
            bool isChangeActive = UseConfig ? _config.IsChangeActive : _changeOptions.Contains(ChangeOption.Active);
            bool portraitActive = UseConfig ? _config.PortraitGameObjectIsActive : _portraitGameObjectIsActive;
            bool landscapeActive = UseConfig ? _config.LandscapeGameObjectIsActive : _landscapeGameObjectIsActive;
            UpdateActive(isChangeActive, isPortrait, portraitActive, landscapeActive);

        }

        private void UpdateActive(bool useProperty, bool isPortrait, bool portraitValue, bool landscapeValue)
        {
            if (useProperty)
            {
                bool objectIsActive = isPortrait ? _portraitGameObjectIsActive : _landscapeGameObjectIsActive;
                gameObject.SetActive(objectIsActive);
            }
        }

        private void UpdateContainer(bool useProperty, bool isPortrait, RectTransform portraitContainer, RectTransform landscapeContainer)
        {
            if (useProperty)
            {
                var container = isPortrait ? portraitContainer : landscapeContainer;
                transform.SetParent(container);
                transform.localPosition = Vector3.zero;
                _portraitContainer.gameObject.SetActive(isPortrait);
                _landscapeContainer.gameObject.SetActive(!isPortrait);
            }
        }

        private void UpdateLocalPosition(bool useProperty, bool isPortrait, Vector2 portraitPosition, Vector2 landscapePosition)
        {
            if (useProperty)
            {
                Vector2 localPosition = isPortrait ? portraitPosition : landscapePosition;
                (transform as RectTransform).anchoredPosition = localPosition;
            }
        }

        private void UpdateScale(bool useProperty, bool isPortrait, float portraitScale, float landscapeScale)
        {
            if (useProperty)
            {
                float scale = isPortrait ? portraitScale : landscapeScale;
                transform.localScale = Vector3.one * scale;
            }
        }

    }
}


