using System.Collections.Generic;
#if UNITY_WEBGL
using Cysharp.Threading.Tasks;
using GamePush;
#endif
using UnityEngine;


namespace DevNote.SDK.GamePush
{
    public class GamePushRemoteService : MonoBehaviour, IRemote
    {
#if UNITY_WEBGL
        private Dictionary<RemoteKey, string> _values = new();
        private bool _initialized = false;

        Dictionary<RemoteKey, string> IRemote.Values => _values;

        bool ISelectableService.IsAvailableForSelection =>
            GamePushEnvironmentService.IsAvailableForSelection;

        bool IInitializable.Initialized => _initialized;

        async void IInitializable.Initialize()
        {
            await UniTask.WaitUntil(() => GP_Init.isReady);

            foreach (var remoteKey in Utils.GetEnumTypes<RemoteKey>())
            {
                var remoteKeyName = remoteKey.ToString();
                if (GP_Variables.Has(remoteKeyName))
                {
                    var value = GP_Variables.GetString(remoteKeyName);
                    _values.Add(remoteKey, value);
                }
            }

            _initialized = true;
        }
#else
        Dictionary<RemoteKey, string> IRemote.Values => null;

        bool ISelectableService.IsAvailableForSelection => false;

        bool IInitializable.Initialized => false;

        void IInitializable.Initialize() { }
#endif
    }
}
