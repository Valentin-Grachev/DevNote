using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    public class TestRemoteService : MonoBehaviour, IRemote
    {
        [Serializable] private struct RemoteData
        {
            public string key;
            public string value;
        }

        [SerializeField] private List<RemoteData> _remoteData;


        bool ISelectableService.IsAvailableForSelection => true;
        bool IInitializable.Initialized => true;
        void IInitializable.Initialize() { }


        string IRemote.GetString(string remoteKey)
        {
            int index = _remoteData.FindIndex((data) => data.key == remoteKey);

            if (index == -1) 
                throw IRemote.UndefinedKeyException(GetType(), remoteKey);

            return _remoteData[index].value;
        }


        
    }
}

