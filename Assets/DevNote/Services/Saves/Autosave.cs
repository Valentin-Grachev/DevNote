using UnityEngine;

namespace DevNote
{
    public class Autosave : MonoBehaviour
    {
        [SerializeField] private float _localSaveCooldown = 1f;
        [SerializeField] private float _cloudSaveCooldown = 60f;

        private ISave save => Context.Get<ISave>();

        private float _timeToLocalSave;
        private float _timeToCloudSave;

        private void Awake()
        {
            WebHandler.onPageBeforeUnload += () => save.SaveLocal();
            WebHandler.onPageHidden += () => save.SaveLocal();
        }

        private void Start()
        {
            _timeToLocalSave = _localSaveCooldown;
            _timeToCloudSave = _cloudSaveCooldown;
        }


        private void Update()
        {
            if (!save.Initialized) return;

            _timeToLocalSave -= Time.unscaledDeltaTime;
            _timeToCloudSave -= Time.unscaledDeltaTime;

            if (_timeToLocalSave < 0f)
            {
                _timeToLocalSave = _localSaveCooldown;
                save.SaveLocal();
            }

            if (_timeToCloudSave < 0f)
            {
                _timeToCloudSave = _cloudSaveCooldown;
                save.SaveCloud();
            }
        }


        private void OnApplicationFocus(bool focus)
        {
            if (!save.Initialized) return;

            if (!focus) save.SaveLocal();
        }

        private void OnApplicationPause(bool pause)
        {
            if (!save.Initialized) return;

            if (pause) save.SaveLocal();
        }

        private void OnApplicationQuit()
        {
            if (!save.Initialized) return;

            save.SaveCloud();
        }




    }
}



