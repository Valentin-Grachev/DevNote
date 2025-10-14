using System;
using UnityEngine;

namespace DevNote
{
    public interface ISave : IInitializable, ISelectableService
    {
        public const string DATA_KEY = "data";

        public static event Action OnSavesDeleted;
        protected static bool SavesDeleted { get; private set; }
        public static DateTime UsedSaveTime { get; protected set; }


        protected static void SetSavesAsDeleted()
        {
            PlayerPrefs.SetString(DATA_KEY, string.Empty);
            PlayerPrefs.Save();

            SavesDeleted = true;
            OnSavesDeleted?.Invoke();
        }


        public sealed void SaveLocal(Action onSuccess = null, Action onError = null)
        {
            if (SavesDeleted) { onError?.Invoke(); return; }

            PlayerPrefs.SetString(DATA_KEY, IGameState.GetEncodedData());
            PlayerPrefs.Save();

            onSuccess?.Invoke();
        }


        public void SaveCloud(Action onSuccess = null, Action onError = null);
        public void DeleteSaves(Action onSuccess = null, Action onError = null);

    }

}

