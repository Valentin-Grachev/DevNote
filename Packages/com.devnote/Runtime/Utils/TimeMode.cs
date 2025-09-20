using System.Collections.Generic;
using UnityEngine;


namespace DevNote
{

    public static class TimeMode
    {
        public enum Mode { Pause, Stop }



        private static Dictionary<Mode, float> priorityTimeScales = new Dictionary<Mode, float>
        {
            { Mode.Stop, 0f },
            { Mode.Pause, 0f},
        };

        private static List<Mode> _activeModes = new();



        public static void SetActive(Mode mode, bool active)
        {
            if (active) _activeModes.Add(mode);
            else _activeModes.Remove(mode);
            UpdateTime();
               
        }

        private static void UpdateTime()
        {
            AudioListener.pause = false;

            foreach (var priorityTimeScale in priorityTimeScales)
            {
                foreach (var mode in _activeModes)
                    if (priorityTimeScale.Key == mode)
                    {
                        Time.timeScale = priorityTimeScale.Value;

                        if (priorityTimeScale.Key == Mode.Stop)
                            AudioListener.pause = true;

                        return;
                    }
            }

            Time.timeScale = 1f;
        }








    }



}


