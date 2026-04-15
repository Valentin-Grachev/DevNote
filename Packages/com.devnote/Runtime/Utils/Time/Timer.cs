using System;
using Cysharp.Threading.Tasks;

namespace DevNote
{
    public class Timer
    {
        private Action _onTick;
        private Action _onFinished;

        public int SecondsLeft { get; private set; } = 0;

        public Timer(int seconds, Action onTick = null, Action onFinished = null, bool ignoreTimeScale = false)
        {
            SecondsLeft = seconds;
            _onTick = onTick;
            _onFinished = onFinished;

            StartTimer(ignoreTimeScale);
        }

        private async void StartTimer(bool ignoreTimeScale)
        {
            while (SecondsLeft > 0)
            {
                await UniTask.Delay(1000, ignoreTimeScale);

                SecondsLeft--;
                _onTick?.Invoke();
            }

            _onFinished?.Invoke();
        }




    }
}
