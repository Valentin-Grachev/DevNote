using System;

namespace DevNote
{
    public class OneShotAction
    {
        private Action _action;

        public OneShotAction(Action action)
        {
            _action = action;
        }


        public void Invoke()
        {
            if (_action == null) return;

            _action?.Invoke();
            _action = null;
        }


    }
}


