using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEditor.Graphs;


#if UNITY_EDITOR
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEditor;
#endif

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




        public static AssetReferenceT<T> MakeAssetAsAddressable<T>(string assetPath, string groupName) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            var settings = AddressableAssetSettingsDefaultObject.Settings;

            string guid = AssetDatabase.AssetPathToGUID(assetPath);

            // 1. Найти группу
            var group = settings.FindGroup(groupName);

            // 2. Если нет — создать
            if (group == null)
            {
                group = settings.CreateGroup(groupName, false, false, false, null,
                    typeof(UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema),
                    typeof(UnityEditor.AddressableAssets.Settings.GroupSchemas.ContentUpdateGroupSchema)
                );
            }

            var entry = settings.CreateOrMoveEntry(guid, group);
            entry.address = assetPath;

            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
            AssetDatabase.SaveAssets();

            return new AssetReferenceT<T>(guid);
#endif

#pragma warning disable CS0162
            return null;
#pragma warning restore CS0162
        }


        public static T RemoveAssetFromAddressables<T>(string guid) where T : UnityEngine.Object
        {
#if UNITY_EDITOR
            var settings = AddressableAssetSettingsDefaultObject.Settings;

            var entry = settings.FindAssetEntry(guid);

            if (entry != null)
            {
                settings.RemoveAssetEntry(guid);
                AssetDatabase.SaveAssets();
            }

            var path = AssetDatabase.GUIDToAssetPath(guid);

            return AssetDatabase.LoadAssetAtPath<T>(path);
#endif

#pragma warning disable CS0162
            return null;
#pragma warning restore CS0162
        }






    }
}
