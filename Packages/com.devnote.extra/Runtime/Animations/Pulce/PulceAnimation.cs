using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace DevNote.Extra
{
    public class PulceAnimation : MonoBehaviour
    {
        [SerializeField, Expandable] private PulceAnimationPreset _preset;

        [HideIf(nameof(UsePreset)), Space, SerializeField] private float _loopDuration = 1f;
        [HideIf(nameof(UsePreset)), SerializeField] private Vector2 _fromToScale;

        private Tween _currentTween;

        private bool UsePreset => _preset != null;
        private float LoopDuration => UsePreset ? _preset.LoopDuration : _loopDuration;
        private Vector2 FromToScale => UsePreset ? _preset.FromToScale : _fromToScale;


        private void OnEnable()
        {
            transform.localScale = Vector3.one * _fromToScale.x;

            _currentTween = transform.DOScale(_fromToScale.y, LoopDuration)
                .SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
        }


        private void OnDisable()
        {
            _currentTween?.Kill();
        }

    }
}
