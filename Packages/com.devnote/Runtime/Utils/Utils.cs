using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DevNote
{
    public static class Utils
    {

        public static async UniTask<bool> AddressableExists(string key)
        {
            var handle = Addressables.LoadResourceLocationsAsync(key);

            try
            {
                await handle.ToUniTask();

                return handle.Status == AsyncOperationStatus.Succeeded
                       && handle.Result != null
                       && handle.Result.Count > 0;
            }
            finally
            {
                Addressables.Release(handle);
            }
        }


        public static List<T> GetEnumTypes<T>() where T : Enum
        {
            var list = new List<T>();
            foreach (var type in Enum.GetValues(typeof(T)))
                list.Add((T)type);

            return list;
        }





    }
}
