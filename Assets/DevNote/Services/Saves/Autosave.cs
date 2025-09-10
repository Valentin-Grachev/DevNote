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
            WebHandler.OnPageBeforeUnload += () => save.Item.SaveLocal();
            WebHandler.OnPageHidden += () => save.Item.SaveLocal();
        }

        private void Start()
        {
            _timeToLocalSave = _localSaveCooldown;
            _timeToCloudSave = _cloudSaveCooldown;
        }


        private void Update()
        {
            if (!save.Item.Initialized) return;

            _timeToLocalSave -= Time.unscaledDeltaTime;
            _timeToCloudSave -= Time.unscaledDeltaTime;

            if (_timeToLocalSave < 0f)
            {
                _timeToLocalSave = _localSaveCooldown;
                save.Item.SaveLocal();
            }

            if (_timeToCloudSave < 0f)
            {
                _timeToCloudSave = _cloudSaveCooldown;
                save.Item.SaveCloud();
            }
        }


        private void OnApplicationFocus(bool focus)
        {
            if (!save.Resolved || !save.Item.Initialized) return;

            if (!focus) save.Item.SaveLocal();
        }

        private void OnApplicationPause(bool pause)
        {
            if (!save.Resolved || !save.Item.Initialized) return;

            if (pause) save.Item.SaveLocal();
        }

        private void OnApplicationQuit()
        {
            if (!save.Resolved || !save.Item.Initialized) return;

            save.Item.SaveCloud();
        }




    }
}



