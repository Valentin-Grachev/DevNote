using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DevNote.SDK.YandexGames
{
    public class YandexGamesRemoteService : MonoBehaviour, IRemote
    {
        private bool _initialized = false;
        private Dictionary<string, string> _values;

        bool ISelectableService.IsAvailableForSelection => YG_Sdk.IsAvailableForSelection;

        bool IInitializable.Initialized => _initialized;

        string IRemote.GetString(string remoteKey)
        {
            if (!_values.ContainsKey(remoteKey))
                throw IRemote.UndefinedKeyException(GetType(), remoteKey);

            return _values[remoteKey];
        }


        async void IInitializable.Initialize()
        {
            await UniTask.WaitUntil(() => YG_Sdk.available);

            YG_Flags.RequestFlagsJson(onFlagsJsonReceived: (json) =>
            {
                Debug.Log($"[{GetType().Name}] Flags JSON: {json}");

                _values = ParseJson(json);
                _initialized = true;
            });
        }

        private Dictionary<string, string> ParseJson(string json)
        {
            var values = new Dictionary<string, string>();

            json = json.Trim().TrimStart('{').TrimEnd('}').Trim();

            if (!string.IsNullOrEmpty(json))
            {
                var pairs = json.Split(',');

                foreach (var pair in pairs)
                {
                    var keyValue = pair.Split(':');

                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0].Trim().Trim('"');
                        string value = keyValue[1].Trim().Trim('"');

                        values[key] = value;
                    }
                }
            }

            return values;
        }



    }

}
