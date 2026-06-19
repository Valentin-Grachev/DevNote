using System;
using UnityEngine;

namespace DevNote
{
    public interface ISave : IInitializable, ISelectableService
    {
        public const string DATA_KEY = "data";
        public const string SAVES_DELETED_KEY = "saves_deleted";

        public static event Action OnSavesDeleted;
        protected static bool SavesDeleted => PlayerPrefs.HasKey(SAVES_DELETED_KEY); 
        public static DateTime UsedSaveTime { get; protected set; }


        protected static void SetSavesAsDeleted()
        {
            PlayerPrefs.SetString(DATA_KEY, string.Empty);
            PlayerPrefs.Save();

            PlayerPrefs.SetInt(SAVES_DELETED_KEY, 1);
            OnSavesDeleted?.Invoke();
        }


        public sealed void SaveLocal(Action onSuccess = null, Action onError = null)
        {
            if (SavesDeleted) { onError?.Invoke(); return; }

            PlayerPrefs.SetString(DATA_KEY, IGameState.GetEncodedData());
            PlayerPrefs.Save();

            onSuccess?.Invoke();
        }

        public sealed void FullSave(Action onSuccess = null, Action onError = null)
        {
            SaveLocal();
            SaveCloud(onSuccess, onError);
        }

        public void SaveCloud(Action onSuccess = null, Action onError = null);
        public void DeleteSaves(Action onSuccess = null, Action onError = null);


        public static bool TryLoadGameStateAfterSaveDelete()
        {
            if (!SavesDeleted) return false;

            PlayerPrefs.DeleteKey(SAVES_DELETED_KEY);
            IGameState.RestoreFromEncodedData(string.Empty);
            UsedSaveTime = DateTime.MinValue;

            return true;
        }



    }

}

