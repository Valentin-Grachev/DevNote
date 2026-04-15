using System;
using Cysharp.Threading.Tasks;

namespace DevNote
{
    public class Timer
    {
        private bool _isStopped = false;
        private Action _onTick;
        private Action _onFinished;

        public int SecondsLeft { get; private set; } = 0;
        public bool IsPaused { get; set; } = false;

        public Timer(int seconds, Action onTick = null, Action onFinished = null, bool ignoreTimeScale = false)
        {
            SecondsLeft = seconds;
            _onTick = onTick;
            _onFinished = onFinished;

            Start(ignoreTimeScale);
        }

        private async void Start(bool ignoreTimeScale)
        {
            while (SecondsLeft > 0)
            {
                await UniTask.Delay(1000, ignoreTimeScale);

                if (IsPaused) continue;
                if (_isStopped) return;

                SecondsLeft--;
                _onTick?.Invoke();
            }

            _onFinished?.Invoke();
        }

        public void Stop()
        {
            _isStopped = true;
            _onFinished.Invoke();
        }



    }
}
