using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using MirraSdk = MirraGames.SDK.MirraSDK;

namespace DevNote.SDK.MirraSDK
{
    public class MirraRemoteService : MonoBehaviour, IRemote
    {
        private readonly Dictionary<RemoteKey, string> _values = new();
        private bool _initialized;

        Dictionary<RemoteKey, string> IRemote.Values => _values;

        bool ISelectableService.IsAvailableForSelection => MirraEnvironmentService.IsAvailableForSelection;

        bool IInitializable.Initialized => _initialized;

        async void IInitializable.Initialize()
        {
            await UniTask.WaitUntil(() => MirraSdk.IsInitialized);

            foreach (var remoteKey in Utils.GetEnumTypes<RemoteKey>())
            {
                var remoteKeyName = remoteKey.ToString();
                if (MirraSdk.Flags.HasKey(remoteKeyName))
                    _values.Add(remoteKey, MirraSdk.Flags.GetString(remoteKeyName));
            }

            _initialized = true;
        }
    }
}
