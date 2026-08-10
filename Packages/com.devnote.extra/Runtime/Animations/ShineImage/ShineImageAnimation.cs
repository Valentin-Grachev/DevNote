using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace DevNote.Extra
{
    public class ShineImageAnimation : MonoBehaviour
    {
        [SerializeField, Expandable] private ShineImageAnimationPreset _preset;

        [HideIf(nameof(UsePreset)), Space, SerializeField] private Vector2 _fromToAlpha;
        [HideIf(nameof(UsePreset)), SerializeField] private float _loopDuration = 1f;

        private Image _image;
        private Tween _currentTween;

        private bool UsePreset => _preset != null;
        private float LoopDuration => UsePreset ? _preset.LoopDuration : _loopDuration;
        private Vector2 FromToAlpha => UsePreset ? _preset.FromToAlpha : _fromToAlpha;




        private void OnEnable()
        {
            _image ??= GetComponent<Image>();

            _image.color = _image.color.SetAlpha(FromToAlpha.x);

            var unfadeTween = DOTween.ToAlpha
                (() => _image.color, x => _image.color = x, FromToAlpha.y, LoopDuration / 2f)
                .SetEase(Ease.InOutFlash).SetUpdate(true);

            var fadeTween = DOTween.ToAlpha
                (() => _image.color, x => _image.color = x, FromToAlpha.x, LoopDuration / 2f)
                .SetEase(Ease.InOutFlash).SetUpdate(true);

            _currentTween?.Kill();
            _currentTween = DOTween.Sequence().Append(unfadeTween).Append(fadeTween)
                .SetLoops(-1);
        }


        private void OnDisable()
        {
            _currentTween?.Kill();
        }



    }
}
