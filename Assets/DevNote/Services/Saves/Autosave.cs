using UnityEngine;

namespace DevNote
{
    public class Autosave : MonoBehaviour
    {
        [SerializeField] private float _localSaveCooldown = 1f;
        [SerializeField] private float _cloudSaveCooldown = 60f;

        private readonly Holder<ISave> save = new();

        private float _timeToLocalSave;
        private float _timeToCloudSave;

        private void Awake()
        {
            WebHandler.onPageBeforeUnload += () => save.Value.SaveLocal();
            WebHandler.onPageHidden += () => save.Value.SaveLocal();
        }

        private void Start()
        {
            _timeToLocalSave = _localSaveCooldown;
            _timeToCloudSave = _cloudSaveCooldown;
        }


        private void Update()
        {
            if (!save.Value.Initialized) return;

            _timeToLocalSave -= Time.unscaledDeltaTime;
            _timeToCloudSave -= Time.unscaledDeltaTime;

            if (_timeToLocalSave < 0f)
            {
                _timeToLocalSave = _localSaveCooldown;
                save.Value.SaveLocal();
            }

            if (_timeToCloudSave < 0f)
            {
                _timeToCloudSave = _cloudSaveCooldown;
                save.Value.SaveCloud();
            }
        }


        private void OnApplicationFocus(bool focus)
        {
            if (!save.Resolved || !save.Value.Initialized) return;

            if (!focus) save.Value.SaveLocal();
        }

        private void OnApplicationPause(bool pause)
        {
            if (!save.Resolved || !save.Value.Initialized) return;

            if (pause) save.Value.SaveLocal();
        }

        private void OnApplicationQuit()
        {
            if (!save.Resolved || !save.Value.Initialized) return;

            save.Value.SaveCloud();
        }




    }
}



