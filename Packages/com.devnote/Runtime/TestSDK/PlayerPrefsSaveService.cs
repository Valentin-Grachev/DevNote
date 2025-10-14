using System;
using UnityEngine;


namespace DevNote.SDK.Test
{
    public class PlayerPrefsSaveService : MonoBehaviour, ISave
    {
        [SerializeField] private AutosaveSettings _autosaveSettings;


        private bool _initialized = false;

        bool ISelectableService.IsAvailableForSelection => true;

        bool IInitializable.Initialized => _initialized;

        void IInitializable.Initialize()
        {
            _autosaveSettings.Initialize();

            var encodedData = PlayerPrefs.GetString(ISave.DATA_KEY, string.Empty);
            ISave.UsedSaveTime = GameStateEncoder.GetSaveTime(encodedData);

            IGameState.RestoreFromEncodedData(encodedData);

            _initialized = true;
        }


        void ISave.DeleteSaves(Action onSuccess, Action onError)
        {
            ISave.SetSavesAsDeleted();
            onSuccess?.Invoke();
        }

        void ISave.SaveCloud(Action onSuccess, Action onError) => onError?.Invoke();



    }
}


