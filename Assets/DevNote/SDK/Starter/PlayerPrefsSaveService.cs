using System;
using NaughtyAttributes;
using UnityEngine;


namespace DevNote.Services.Test
{
    public class PlayerPrefsSaveService : MonoBehaviour, ISave
    {
        public event Action OnSavesDeleted;

        private bool _initialized = false;
        private bool _savesWasDeleted = false;

        private const string DATA_KEY = "data";



        bool ISelectableService.Available => true;

        bool IProjectInitializable.Initialized => _initialized;

        void IProjectInitializable.Initialize()
        {
            var encodedData = PlayerPrefs.GetString(DATA_KEY, string.Empty);
            GameState.RestoreFromEncodedData(encodedData);

            _initialized = true;
        }


        void ISave.DeleteSaves(Action onSuccess, Action onError)
        {
            PlayerPrefs.SetString(DATA_KEY, string.Empty);
            PlayerPrefs.Save();

            onSuccess?.Invoke();
            OnSavesDeleted?.Invoke();

            _savesWasDeleted = true;
        }

        void ISave.SaveCloud(Action onSuccess, Action onError) => Save(onSuccess, onError);

        void ISave.SaveLocal(Action onSuccess, Action onError) => Save(onSuccess, onError);

        private void Save(Action onSuccess, Action onError)
        {
            if (_savesWasDeleted)
            {
                onError?.Invoke();
                return;
            }

            PlayerPrefs.SetString(DATA_KEY, GameState.GetEncodedData());
            PlayerPrefs.Save();

            onSuccess?.Invoke();
        }



        [Button("Clear Player Prefs")]
        private void ClearData() => PlayerPrefs.DeleteAll();





    }
}


