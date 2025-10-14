using System;
using Cysharp.Threading.Tasks;
using GamePush;
using UnityEngine;


namespace DevNote.SDK.GamePush
{
    public class GamePushSaveService : MonoBehaviour, ISave
    {
        [SerializeField] private AutosaveSettings _autosaveSettings;

        private bool _initialized = false;
        private Action _onSuccess;
        private Action _onError;


        bool IInitializable.Initialized => _initialized;

        bool ISelectableService.IsAvailableForSelection => GamePushEnvironmentService.IsAvailableForSelection;

        async void IInitializable.Initialize()
        {
            _autosaveSettings.Initialize();

            await UniTask.WaitUntil(() => GP_Init.isReady);

            GP_Player.OnSyncComplete += () => _onSuccess?.Invoke();
            GP_Player.OnSyncError += () => _onError?.Invoke();

            var cloudData = GP_Player.GetString(ISave.DATA_KEY);
            var localData = PlayerPrefs.GetString(ISave.DATA_KEY);

            Debug.Log($"[{nameof(GamePushSaveService)}] Cloud data: {cloudData}");
            Debug.Log($"[{nameof(GamePushSaveService)}] Local data: {localData}");

            var cloudTime = GameStateEncoder.GetSaveTime(cloudData);
            var localTime = GameStateEncoder.GetSaveTime(localData);

            bool useCloud = cloudTime > localTime;
            string data = useCloud ? cloudData : localData;

            ISave.UsedSaveTime = GameStateEncoder.GetSaveTime(data);
            IGameState.RestoreFromEncodedData(data);

            Debug.Log($"[{nameof(GamePushSaveService)}] Using cloud: {useCloud}");

            _initialized = true; 
        }

        void ISave.SaveCloud(Action onSuccess, Action onError)
        {
            _onSuccess = onSuccess;
            _onError = onError;

            string data = IGameState.GetEncodedData();
            GP_Player.Set(ISave.DATA_KEY, data);
            GP_Player.Sync();
        }


        void ISave.DeleteSaves(Action onSuccess, Action onError)
        {
            _onError = onError;
            _onSuccess = () =>
            {
                ISave.SetSavesAsDeleted();
                onSuccess?.Invoke();
            };

            GP_Player.Set(ISave.DATA_KEY, string.Empty);
            GP_Player.Sync();
        }
    }
}

