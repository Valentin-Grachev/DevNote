using System.Runtime.InteropServices;
using UnityEngine;


namespace DevNote.SDK.YandexGames
{
    public class YG_Leaderboards : MonoBehaviour
    {

        public static void SetLeaderboardScore(string leaderboardName, int score)
        {
#if UNITY_WEBGL
            _SetLeaderboardScore(leaderboardName, score);
#endif
        }


#if UNITY_WEBGL
        [DllImport("__Internal")] private static extern void _SetLeaderboardScore(string leaderboardName, int score);
#endif

    }
}


