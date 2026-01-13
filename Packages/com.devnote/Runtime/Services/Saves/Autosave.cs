using System;
using UnityEngine;

namespace DevNote
{
    [Serializable] public class AutosaveSettings
    {
        [SerializeField] private Autosave _autosave;

        [Space]
        [SerializeField] private int _localSaveCooldown;
        [SerializeField] private int _cloudSaveCooldown;

        public void Initialize()
        {
            float localSaveCooldown = _localSaveCooldown > 0 ? _localSaveCooldown : float.MaxValue;
            float cloudSaveCooldonw = _cloudSaveCooldown > 0 ? _cloudSaveCooldown : float.MaxValue;

            _autosave.SetCooldowns(localSaveCooldown, cloudSaveCooldonw);
        }
    }



        public class Autosave : MonoBehaviour
    {
        private float _localSaveCooldown = 1f;
        private float _cloudSaveCooldown = 60f;

        private float _timeToLocalSave;
        private float _timeToCloudSave;

        private readonly Holder<ISave> save = new();


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


        public void SetCooldowns(float localSaveCooldown, float cloudSaveCooldown)
        {
            _timeToLocalSave = _localSaveCooldown = localSaveCooldown;
            _timeToCloudSave = _cloudSaveCooldown = cloudSaveCooldown;
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



