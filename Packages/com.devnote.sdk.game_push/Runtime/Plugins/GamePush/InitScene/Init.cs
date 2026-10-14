using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_WEBGL
using GamePush;
#endif

namespace GamePush.Initialization
{
    public class Init : MonoBehaviour
    {
        private async void Start()
        {
#if UNITY_WEBGL
            await GP_Init.Ready;
            SceneManager.LoadScene(1);
#endif
        }
    }
}
