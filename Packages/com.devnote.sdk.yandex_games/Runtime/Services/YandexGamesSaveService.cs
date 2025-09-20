using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DevNote.SDK.YandexGames
{
    public class YandexGamesSaveService : MonoBehaviour, ISave
    {
        private bool _initialized = false;

        
        bool ISelectableService.IsAvailableForSelection => YG_Sdk.ServicesIsSupported;

        bool IInitializable.Initialized => _initialized;


        private readonly Holder<IEnvironment> environment = new();


        async void IInitializable.Initialize()
        {
            await UniTask.WaitUntil(() => YG_Saves.available);
            YG_Saves.InitializePlayer();

            YG_Saves.RequestSaves((cloudData) =>
            {
                var localData = PlayerPrefs.GetString(ISave.DATA_KEY);

                Debug.Log($"[{nameof(YandexGamesSaveService)}] Cloud data: {cloudData}");
                Debug.Log($"[{nameof(YandexGamesSaveService)}] Local data: {localData}");

                var cloudTime = GameStateEncoder.GetSaveTime(cloudData);
                var localTime = GameStateEncoder.GetSaveTime(localData);
                

                bool useCloud = cloudTime > localTime;
                string data = useCloud ? cloudData : localData;

                ISave.UsedSaveTime = GameStateEncoder.GetSaveTime(data);
                IGameState.RestoreFromEncodedData(data);

                Debug.Log($"[{nameof(YandexGamesSaveService)}] Using cloud: {useCloud}");

                _initialized = true;
            });

        }


        void ISave.SaveLocal(Action onSuccess, Action onError)
        {
            if (ISave.SavesDeleted) { onError?.Invoke(); return; }

            PlayerPrefs.SetString(ISave.DATA_KEY, IGameState.GetEncodedData());
            PlayerPrefs.Save();

            onSuccess?.Invoke();
        }

        void ISave.SaveCloud(Action onSuccess, Action onError)
        {
            if (ISave.SavesDeleted) { onError?.Invoke(); return; }

            YG_Saves.SendSaves(IGameState.GetEncodedData(), onSavesSent: (success) =>
            {
                if (success) onSuccess?.Invoke();
                else onError?.Invoke();
            });
        }

        void ISave.DeleteSaves(Action onSuccess, Action onError)
        {
            YG_Saves.SendSaves(string.Empty, onSavesSent: (success) =>
            {
                if (success)
                {
                    PlayerPrefs.SetString(ISave.DATA_KEY, string.Empty);
                    PlayerPrefs.Save();

                    onSuccess?.Invoke();
                    ISave.SetSavesAsDeleted();
                }
                else onError?.Invoke();
            });
        }

        
    }
}


