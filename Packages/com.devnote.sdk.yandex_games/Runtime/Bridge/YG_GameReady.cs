using System.Runtime.InteropServices;
using UnityEngine;


namespace DevNote.SDK.YandexGames
{
    public class YG_GameReady : MonoBehaviour
    {

        public static void GameReady()
        {
#if UNITY_WEBGL
            _GameReady();
#endif
        }


#if UNITY_WEBGL
        [DllImport("__Internal")] private static extern void _GameReady();
#endif


    }


}
    
