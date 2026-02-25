using UnityEngine;

namespace DevNote
{
    public class Clock : IUpdateHandler
    {
        public delegate void OnSecondsPass(int seconds);
        public event OnSecondsPass OnUnscaledSecondsPassed;


        private float _lastUpdateTime = 0f;


        void IUpdateHandler.Update()
        {
            if (Time.unscaledTime >= _lastUpdateTime + 1f)
            {
                int passedSeconds = (int)(Time.unscaledTime - _lastUpdateTime);
                _lastUpdateTime = Time.unscaledTime;

                OnUnscaledSecondsPassed?.Invoke(passedSeconds);
            }
        }


    }
}
