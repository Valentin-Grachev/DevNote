using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



namespace DevNote
{
    public class GoogleTables : MonoBehaviour, IInitializable
    {
        public static bool Initialized => _instance != null && _instance._initialized;

        private enum RequestDataMode { None, Editor, EditorAndBuild }

        private static GoogleTables _instance;


        [SerializeField] private RequestDataMode _startGameRequestMode; 
        [SerializeField] private List<Table> _tables;
        [SerializeField] private List<LoadableFromTable> _loadables;
        [Space(10)]
        [SerializeField] private TableKey _openTable;


        private Dictionary<TableKey, Table> _tablesDictionary;
        private bool _initialized = false;


        

        bool IInitializable.Initialized => _initialized;
        async void IInitializable.Initialize()
        {
            _instance = this;

            switch (_startGameRequestMode)
            {
                case RequestDataMode.None:
                    _initialized = true;
                    break;

                case RequestDataMode.Editor:
                    if (IEnvironment.IsEditor) await RequestData();
                    else _initialized = true;
                    break;

                case RequestDataMode.EditorAndBuild:
                    await RequestData();
                    break;
            }
        }


        [Button("Open Table")]
        private void OpenTableURL() => _tables.Find(table => table.Key == _openTable).OpenTable();



        [Button("Load Data")]
        private async UniTask RequestData()
        {
            gameObject.SetActive(true);
            _tablesDictionary = new();

            foreach (var table in _tables)
            {
                _tablesDictionary.Add(table.Key, table);
                table.RequestData().Forget();
            }

            await WaitAndLoadData();
        }


        private async UniTask WaitAndLoadData()
        {
            var status = Table.LoadingStatus.Loading;

            await UniTask.WaitUntil(() =>
            {
                status = Table.LoadingStatus.Success;

                foreach (var table in _tables)
                {
                    switch (table.Status)
                    {
                        case Table.LoadingStatus.Loading:
                            status = Table.LoadingStatus.Loading;
                            break;

                        case Table.LoadingStatus.Error:
                            throw new Exception($"{Info.Prefix} Table loading error: {table.Key}");
                    }
                }

                return status == Table.LoadingStatus.Success;
            });

            foreach (var loadable in _loadables)
            {
#if UNITY_EDITOR
                EditorUtility.SetDirty(loadable);
#endif
                loadable.LoadData(_tablesDictionary);
            }

#if UNITY_EDITOR
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
#endif

            _initialized = true;
        }

        
    }

}



