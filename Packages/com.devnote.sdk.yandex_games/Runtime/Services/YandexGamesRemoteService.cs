using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DevNote.SDK.YandexGames
{
    public class YandexGamesRemoteService : MonoBehaviour, IRemote
    {
        private bool _initialized = false;
        private Dictionary<RemoteKey, string> _values;

        bool ISelectableService.IsAvailableForSelection => YG_Sdk.IsAvailableForSelection;

        bool IInitializable.Initialized => _initialized;

        Dictionary<RemoteKey, string> IRemote.Values => _values;


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

        private Dictionary<RemoteKey, string> ParseJson(string json)
        {
            var values = new Dictionary<RemoteKey, string>();

            json = json.Trim().TrimStart('{').TrimEnd('}').Trim();

            if (!string.IsNullOrEmpty(json))
            {
                var pairs = json.Split(',');

                foreach (var pair in pairs)
                {
                    var keyValue = pair.Split(':');

                    if (keyValue.Length == 2)
                    {
                        var key = keyValue[0].Trim().Trim('"').ToEnum<RemoteKey>();
                        string value = keyValue[1].Trim().Trim('"');

                        values[key] = value;
                    }
                }
            }

            return values;
        }



    }

}
