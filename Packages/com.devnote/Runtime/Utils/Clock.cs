using UnityEngine;

namespace DevNote
{
    public class Clock : IUpdateHandler
    {
        public delegate void OnTimePassed(int seconds);
        public static event OnTimePassed OnUnscaledSecondsPassed;
        public static event OnTimePassed OnSecondsPassed;


        private float _lastUnscaledUpdateTime = 0f;
        private float _lastUpdateTime = 0f;


        void IUpdateHandler.Update()
        {
            if (Time.unscaledTime >= _lastUnscaledUpdateTime + 1f)
            {
                int passedSeconds = (int)(Time.unscaledTime - _lastUnscaledUpdateTime);
                _lastUnscaledUpdateTime = Time.unscaledTime;

                OnUnscaledSecondsPassed?.Invoke(passedSeconds);
            }

            if (Time.time >= _lastUpdateTime + 1f)
            {
                int passedSeconds = (int)(Time.time - _lastUpdateTime);
                _lastUpdateTime = Time.time;

                OnSecondsPassed?.Invoke(passedSeconds);
            }



        }


    }
}
