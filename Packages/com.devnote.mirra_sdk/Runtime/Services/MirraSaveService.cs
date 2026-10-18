using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using MirraSdk = MirraGames.SDK.MirraSDK;

namespace DevNote.SDK.MirraSDK
{
    public class MirraSaveService : MonoBehaviour, ISave
    {
        [SerializeField] private string _actualSaveData;
        [SerializeField] private bool _useLocalSaves;
        [SerializeField] private AutosaveSettings _autosaveSettings;

        private bool _initialized;

        private string DataKey => string.IsNullOrEmpty(_actualSaveData) ? ISave.DATA_KEY : _actualSaveData;

        bool IInitializable.Initialized => _initialized;

        bool ISelectableService.IsAvailableForSelection => MirraEnvironmentService.IsAvailableForSelection;

        async void IInitializable.Initialize()
        {
            _autosaveSettings.Initialize();

            await UniTask.WaitUntil(() => MirraSdk.IsInitialized);

            if (ISave.TryLoadGameStateAfterSaveDelete())
            {
                _initialized = true;
                Debug.Log($"[{nameof(MirraSaveService)}] Reload after saves delete");
                return;
            }

            var cloudData = MirraSdk.Data.GetString(DataKey);
            var localData = PlayerPrefs.GetString(ISave.DATA_KEY, defaultValue: string.Empty);

            Debug.Log($"[{nameof(MirraSaveService)}] Cloud data: {cloudData}");
            Debug.Log($"[{nameof(MirraSaveService)}] Local data: {localData}");

            var cloudTime = GameStateEncoder.GetSaveTime(cloudData);
            var localTime = GameStateEncoder.GetSaveTime(localData);

            bool useCloud = cloudTime >= localTime || !_useLocalSaves || !IGameState.DataIsSupported(cloudData);
            string data = useCloud ? cloudData : localData;

            ISave.UsedSaveTime = GameStateEncoder.GetSaveTime(data);
            IGameState.RestoreFromEncodedData(data);

            Debug.Log($"[{nameof(MirraSaveService)}] Using cloud: {useCloud}");

            _initialized = true;
        }

        void ISave.SaveCloud(Action onSuccess, Action onError)
        {
            if (ISave.SavesDeleted)
            {
                onError?.Invoke();
                return;
            }

            MirraSdk.Data.SetString(DataKey, IGameState.GetEncodedData());
            MirraSdk.Data.Save();
            onSuccess?.Invoke();
        }

        void ISave.DeleteSaves(Action onSuccess, Action onError)
        {
            MirraSdk.Data.SetString(DataKey, string.Empty);
            MirraSdk.Data.Save();
            ISave.SetSavesAsDeleted();
            onSuccess?.Invoke();
        }
    }
}
