using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    public class TestRemoteService : MonoBehaviour, IRemote
    {
        [Serializable] private struct RemoteData
        {
            public RemoteKey key;
            public string value;
        }

        [SerializeField] private List<RemoteData> _remoteData;

        private Dictionary<RemoteKey, string> _values;

        bool ISelectableService.IsAvailableForSelection => true;
        bool IInitializable.Initialized => true;

        Dictionary<RemoteKey, string> IRemote.Values => _values;

        void IInitializable.Initialize() 
        {
            _values = new();
            foreach (var remoteData in _remoteData)
                _values.Add(remoteData.key, remoteData.value);
        }

        
    }
}

