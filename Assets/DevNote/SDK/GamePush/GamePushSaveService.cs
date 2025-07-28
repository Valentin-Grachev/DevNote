using System;
using Cysharp.Threading.Tasks;
using GamePush;
using UnityEngine;


namespace DevNote.Services.GamePush
{
    public class GamePushSaveService : MonoBehaviour, ISave
    {
        public event Action onSavesDeleted;

        private bool _initialized = false;


        private const string DATA_KEY = "data";
        private const string LAST_SAVE_TIME_KEY = "time";


        private readonly Holder<IEnvironment> environment = new();


        bool ISelectableService.Available => GamePushEnvironmentService.ServicesIsAvailable;


        bool IProjectInitializable.Initialized => _initialized;
        async void IProjectInitializable.Initialize()
        {
            await UniTask.WaitUntil(() => GP_Init.isReady);

            DateTime lastLocalSaveTime = PlayerPrefs.HasKey(LAST_SAVE_TIME_KEY) ? 
                DateTime.Parse(PlayerPrefs.GetString(LAST_SAVE_TIME_KEY)) : DateTime.MinValue;

            DateTime lastCloudSaveTime = GP_Player.GetString(LAST_SAVE_TIME_KEY) != "0" ?
                DateTime.Parse(GP_Player.GetString(LAST_SAVE_TIME_KEY)) : DateTime.MinValue;

            bool useCloudLoading = lastCloudSaveTime > lastLocalSaveTime;

            string data = PlayerPrefs.GetString(DATA_KEY, string.Empty);

            if (useCloudLoading)
            {
                string cloudData = GP_Player.GetString(DATA_KEY);
                data = cloudData != "0" ? cloudData : string.Empty;
            }

            GameState.RestoreFromEncodedData(data);

            _initialized = true;
        }

        void ISave.SaveCloud(Action onSuccess, Action onError)
        {
            var encodedData = GameState.GetEncodedData();

            GP_Player.Set(DATA_KEY, encodedData);
            GP_Player.Set(LAST_SAVE_TIME_KEY, environment.Value.ServerTime.ToString());
            GP_Player.Sync();

            onSuccess?.Invoke();
        }

        void ISave.SaveLocal(Action onSuccess, Action onError)
        {
            var encodedData = GameState.GetEncodedData();

            PlayerPrefs.SetString(DATA_KEY, encodedData);
            PlayerPrefs.SetString(LAST_SAVE_TIME_KEY, environment.Value.ServerTime.ToString());
            PlayerPrefs.Save();

            onSuccess?.Invoke();
        }

        void ISave.DeleteSaves(Action onSuccess, Action onError)
        {
            PlayerPrefs.SetString(DATA_KEY, string.Empty);
            PlayerPrefs.SetString(LAST_SAVE_TIME_KEY, environment.Value.ServerTime.ToString());

            GP_Player.Set(DATA_KEY, string.Empty);
            GP_Player.Set(LAST_SAVE_TIME_KEY, environment.Value.ServerTime.ToString());

            PlayerPrefs.Save();
            GP_Player.Sync();

            onSavesDeleted?.Invoke();
        }
    }
}



