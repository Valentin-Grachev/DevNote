using System;
using Cysharp.Threading.Tasks;
using DevNote.YandexGamesSDK;
using UnityEngine;

namespace DevNote.Services.YandexGames
{
    public class YandexGamesSaveService : MonoBehaviour, ISave
    {
        public event Action OnSavesDeleted;


        private bool _initialized = false;

        private const string LOCAL_DATA_KEY = "data";

        
        bool ISelectableService.Available => YG_Sdk.ServicesIsSupported;

        bool IInitializable.Initialized => _initialized;

        async void IInitializable.Initialize()
        {
            await UniTask.WaitUntil(() => YG_Saves.available);
            YG_Saves.InitializePlayer();

            YG_Saves.RequestSaves((savedData) =>
            {
                GameState.RestoreFromEncodedData(savedData);
                _initialized = true;
            });

        }


        void ISave.SaveLocal(Action onSuccess, Action onError)
        {
            PlayerPrefs.SetString(LOCAL_DATA_KEY, GameState.GetEncodedData());
            PlayerPrefs.Save();

            onSuccess?.Invoke();
        }

        void ISave.SaveCloud(Action onSuccess, Action onError)
        {
            YG_Saves.SendSaves(GameState.GetEncodedData(), onSavesSent: (success) =>
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
                    onSuccess?.Invoke();
                    OnSavesDeleted?.Invoke();
                }
                else onError?.Invoke();
            });
        }

        
    }
}


