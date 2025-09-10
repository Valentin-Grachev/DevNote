using System;

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
            SavesDeleted = true;
            OnSavesDeleted?.Invoke();
        }


        public void SaveLocal(Action onSuccess = null, Action onError = null);
        public void SaveCloud(Action onSuccess = null, Action onError = null);
        public void DeleteSaves(Action onSuccess = null, Action onError = null);

    }

}

