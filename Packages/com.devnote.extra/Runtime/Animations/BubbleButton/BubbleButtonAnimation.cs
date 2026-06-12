using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DevNote.Extra
{
    [RequireComponent(typeof(Button))]
    public class BubbleButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform _targetRect;
        [SerializeField, Expandable] private BubbleButtonAnimationPreset _preset;

        [HideIf(nameof(UsePreset)), Space, SerializeField] private SoundUnit _clickSound;
        [HideIf(nameof(UsePreset)), SerializeField] private SoundUnit _pointerEnterSound;
        [HideIf(nameof(UsePreset)), SerializeField] private SoundUnit _pointerExitSound;

        [HideIf(nameof(UsePreset)), Space, SerializeField] private float _highlightScale;
        [HideIf(nameof(UsePreset)), SerializeField] private float _highlightDuration;

        [HideIf(nameof(UsePreset)), Space, SerializeField] private float _clickScale;
        [HideIf(nameof(UsePreset)), SerializeField] private float _clickDuration;

        private Button _button;
        private Tween _pointerTween;
        private Tween _clickTween;

        private bool UsePreset => _preset != null;


        private SoundUnit ClickSound => UsePreset ? _preset.ClickSound : _clickSound;
        private SoundUnit PointerEnterSound => UsePreset ? _preset.PointerEnterSound : _pointerEnterSound;
        private SoundUnit PointerExitSound => UsePreset ? _preset.PointerExitSound : _pointerExitSound;
        private float HighlightScale => UsePreset ? _preset.HighlightScale : _highlightScale;
        private float HighlightDuration => UsePreset ? _preset.HighlightDuration : _highlightDuration;
        private float ClickScale => UsePreset ? _preset.ClickScale : _clickScale;
        private float ClickDuration => UsePreset ? _preset.ClickDuration : _clickDuration;


        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _pointerTween?.Kill();
            _clickTween?.Kill();
        }


        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            if (!_button.interactable) return;

            PointerEnterSound?.Play();

            _pointerTween?.Kill();
            _pointerTween = _targetRect.DOScale(HighlightScale, HighlightDuration)
                .SetEase(Ease.OutFlash).SetUpdate(true);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            if (!_button.interactable) return;

            PointerExitSound?.Play();

            _pointerTween?.Kill();
            _pointerTween = _targetRect.DOScale(1f, HighlightDuration)
                .SetEase(Ease.OutFlash).SetUpdate(true);
        }


        private void OnButtonClick()
        {
            ClickSound?.Play();

            _button.interactable = false;

            _pointerTween?.Kill();
            _clickTween?.Kill();

            _clickTween = DOTween.Sequence().SetUpdate(true)
                .Append(_targetRect.DOScale(ClickScale, ClickDuration / 2f).SetEase(Ease.OutQuad))
                .Append(_targetRect.DOScale(1f, ClickDuration / 2f).SetEase(Ease.InQuad))
                .OnComplete(() =>
                {
                    _button.interactable = true;
                });
        }

    }
}
