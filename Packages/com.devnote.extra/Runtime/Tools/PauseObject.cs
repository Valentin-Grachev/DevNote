using UnityEngine;

namespace DevNote.Extra
{
    public class PauseObject : MonoBehaviour
    {
        private void OnEnable()
        {
            TimeMode.SetActive(TimeMode.Mode.Pause, true);
        }

        private void OnDisable()
        {
            TimeMode.SetActive(TimeMode.Mode.Pause, false);
        }
    }
}
