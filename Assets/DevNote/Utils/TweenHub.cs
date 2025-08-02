using DG.Tweening;
using UnityEngine;

namespace DevNote
{

    public static class TweenHub
    {

        private const float SHOW_WINDOW_DURATION = 0.3f;
        private const float SHOW_WINDOW_FROM_ROTATION_Z = -30f;
        private const float HIDE_WINDOW_DURATION = 0.2f;


        private const float POP_DURATION = 0.22f;
        private const float POP_DOWN_TO_SCALE = 0.8f;
        private const float POP_UP_TO_SCALE = 1.25f;


        public static Tween PopDown(Transform transform) => DOTween.Sequence()
            .Append(transform.DOScale(POP_DOWN_TO_SCALE, POP_DURATION / 2f))
            .Append(transform.DOScale(1f, POP_DURATION / 2f).SetEase(Ease.OutBack));

        public static Tween PopUp(Transform transform) => DOTween.Sequence()
            .Append(transform.DOScale(POP_UP_TO_SCALE, POP_DURATION / 2f).SetEase(Ease.OutFlash))
            .Append(transform.DOScale(1f, POP_DURATION / 2f).SetEase(Ease.OutBack));

        public static Tween ShowWindow(Transform transform)
        {
            transform.localScale = Vector3.zero;
            transform.rotation = Quaternion.Euler(0f, 0f, SHOW_WINDOW_FROM_ROTATION_Z);

            return DOTween.Sequence()
                .Append(transform.DOScale(1f, SHOW_WINDOW_DURATION).SetEase(Ease.OutBack))
                .Join(transform.DORotate(Vector3.zero, SHOW_WINDOW_DURATION).SetEase(Ease.OutBack));
        }

        public static Tween HideWindow(Transform transform)
        {
            var toRotation = new Vector3(0f, 0f, SHOW_WINDOW_FROM_ROTATION_Z);

            return DOTween.Sequence()
                .Append(transform.DOScale(0f, HIDE_WINDOW_DURATION).SetEase(Ease.OutFlash))
                .Join(transform.DORotate(toRotation, HIDE_WINDOW_DURATION).SetEase(Ease.OutFlash));
        }


    }
}


