using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace DevNote
{
    public enum Column 
    { 
        A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z
    }


    [System.Serializable]
    public class Table
    {
        public enum LoadingStatus { Loading, Success, Error }


        [field: SerializeField] public TableKey Key { get; private set; }
        [Space(10)]
        [SerializeField] private string _webId;
        [SerializeField] private string _gid;

        public LoadingStatus Status { get; private set; }

        private List<List<string>> _cells;

        private string OpenURL => OPEN_URL_TEMPLATE.Replace("*", _webId) + "#gid=" + _gid;
        private const string OPEN_URL_TEMPLATE = "https://docs.google.com/spreadsheets/d/*/edit";

        public string LoadURL => LOAD_URL_TEMPLATE.Replace("*", _webId) + "&gid=" + _gid;
        private const string LOAD_URL_TEMPLATE = "https://docs.google.com/spreadsheets/d/*/export?format=csv";


        public void OpenTable() => Application.OpenURL(OpenURL);


        public string Get(int row, Column column) => _cells[row - 1][(int)column];

        public int Rows => _cells.Count;
        public int Columns => _cells[0].Count;


        public async UniTask RequestData()
        {
            Status = LoadingStatus.Loading;

            using (UnityWebRequest request = UnityWebRequest.Get(LoadURL))
            {
                await request.SendWebRequest().ToUniTask();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string csvData = request.downloadHandler.text;
                    _cells = CsvParcer.Parce(csvData);

                    Status = LoadingStatus.Success;
                    Debug.Log($"{Info.Prefix} Table {Key} Success");
                }
                else
                {
                    Status = LoadingStatus.Error;
                    Debug.LogError($"{Info.Prefix} Table {Key} Error: " + request.error);
                }
            }

            await UniTask.Yield(); // Для сохранения асинхронного контекста
        }


        



    }
}



