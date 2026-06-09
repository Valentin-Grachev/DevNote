using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;

namespace DevNote.Extra
{
    public class RotationAnimation : MonoBehaviour
    {
        [SerializeField, Expandable] private RotationAnimationPreset _preset;

        [HideIf(nameof(UsePreset)), Space, SerializeField] private float _loopDuration = 1f;

        private Tween _currentTween;

        private bool UsePreset => _preset != null;
        private float LoopDuration => UsePreset ? _preset.LoopDuration : _loopDuration;


        private void OnEnable()
        {
            transform.localRotation = Quaternion.Euler(Vector3.zero);

            _currentTween = transform.DOLocalRotate(new Vector3(0, 0, -360f), LoopDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart).SetUpdate(true);
        }


        private void OnDisable()
        {
            _currentTween?.Kill();
        }


    }

}


