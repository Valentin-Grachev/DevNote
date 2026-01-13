using System;
using UnityEngine;

#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif


namespace DevNote.SDK.YandexGames
{
    public class YG_Flags : MonoBehaviour
    {
        private static Action<string> _onFlagsJsonReceived;


        public static void RequestFlagsJson(Action<string> onFlagsJsonReceived)
        {
            _onFlagsJsonReceived = onFlagsJsonReceived;

#if UNITY_WEBGL
            _RequestFlagsJson();
#endif
        }

#if UNITY_WEBGL
        [DllImport("__Internal")] private static extern string _RequestFlagsJson();
#endif


        private void JS_OnFlagsJsonReceived(string json) => _onFlagsJsonReceived?.Invoke(json);


    }
}

