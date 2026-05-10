using System;
using Cysharp.Threading.Tasks;
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

        public static TimeSpan OfflineTime { get; private set; }


        public async UniTask Initialize(ISave save, IEnvironment environment)
        {
            await UniTask.WaitUntil(() => save.Initialized && environment.Initialized);

            OfflineTime = IGameState.IsFirstLaunch ?
                TimeSpan.Zero : IEnvironment.UtcTime - IGameState.LastOnlineTime;
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
