using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DevNote
{
    public class GameTime : IUpdateHandler
    {
        public static event Action OnInitialized;

        public delegate void OnTimePassed(int seconds);
        public static event OnTimePassed OnUnscaledSecondsPassed;
        public static event OnTimePassed OnSecondsPassed;

        private float _lastUnscaledUpdateTime = 0f;
        private float _lastUpdateTime = 0f;

        public static bool IsFirstLaunch { get; private set; }
        public static TimeSpan OfflineTime { get; private set; }


        public void Initialize(ISave save, IEnvironment environment)
        {
            OfflineTime = IGameState.IsFirstLaunch ?
                TimeSpan.Zero : IEnvironment.UtcTime - IGameState.LastOnlineTime;

            IsFirstLaunch = IGameState.IsFirstLaunch;

            IGameState.IsFirstLaunch = false;
            IGameState.LastOnlineTime = IEnvironment.UtcTime;

            OnInitialized?.Invoke();
        }


        void IUpdateHandler.Update()
        {
            if (Time.unscaledTime >= _lastUnscaledUpdateTime + 1f)
            {
                int passedSeconds = (int)(Time.unscaledTime - _lastUnscaledUpdateTime);
                _lastUnscaledUpdateTime = Time.unscaledTime;

                IGameState.LastOnlineTime = IEnvironment.UtcTime;

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
