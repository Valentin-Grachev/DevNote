using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace DevNote
{
    public class OrientationAdapter : MonoBehaviour
    {
        private enum ChangeOption { Scale, Container, LocalPosition, Active }


        [BoxGroup("Active:"), ShowIf(nameof(IsChangeActive)), SerializeField, Label("Is Active In Portrait")] private bool _portraitGameObjectIsActive = true;
        [BoxGroup("Active:"), ShowIf(nameof(IsChangeActive)), SerializeField, Label("Is Active In Landscape")] private bool _landscapeGameObjectIsActive = true;

        [BoxGroup("Container:"), ShowIf(nameof(IsChangeContainer)), SerializeField, Label("Portrait Container")] private RectTransform _portraitContainer;
        [BoxGroup("Container:"), ShowIf(nameof(IsChangeContainer)), SerializeField, Label("Landscape Container")] private RectTransform _landscapeContainer;

        [BoxGroup("Local Position:"), ShowIf(nameof(IsChangeLocalPosition)), SerializeField, Label("Portrait Position")] private Vector2 _portraitLocalPosition;
        [BoxGroup("Local Position:"), ShowIf(nameof(IsChangeLocalPosition)), SerializeField, Label("Landscape Position")] private Vector2 _landscapeLocalPosition;

        [BoxGroup("Scale:"), ShowIf(nameof(IsChangeScale)), SerializeField, Label("Portrait Scale")] private float _portraitScale;
        [BoxGroup("Scale:"), ShowIf(nameof(IsChangeScale)), SerializeField, Label("Landscape Scale")] private float _landscapeScale;

        [BoxGroup(""), SerializeField] private List<ChangeOption> _changeOptions = new();

        private bool IsChangeScale => _changeOptions.Contains(ChangeOption.Scale);
        private bool IsChangeContainer => _changeOptions.Contains(ChangeOption.Container);
        private bool IsChangeLocalPosition => _changeOptions.Contains(ChangeOption.LocalPosition);
        private bool IsChangeActive => _changeOptions.Contains(ChangeOption.Active);






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

            if (IsChangeActive)
            {
                bool objectIsActive = isPortrait ? _portraitGameObjectIsActive : _landscapeGameObjectIsActive;
                gameObject.SetActive(objectIsActive);
            }

            if (IsChangeContainer)
            {
                var container = isPortrait ? _portraitContainer : _landscapeContainer;
                transform.SetParent(container);
                transform.localPosition = Vector3.zero;
                _portraitContainer.gameObject.SetActive(isPortrait);
                _landscapeContainer.gameObject.SetActive(!isPortrait);
            }

            if (IsChangeLocalPosition)
            {
                Vector2 localPosition = isPortrait ? _portraitLocalPosition : _landscapeLocalPosition;
                transform.localPosition = localPosition;
            }
            
            if (IsChangeScale)
            {
                float scale = isPortrait ? _portraitScale : _landscapeScale;
                transform.localScale = Vector3.one * scale;
            }

        }




    }
}


