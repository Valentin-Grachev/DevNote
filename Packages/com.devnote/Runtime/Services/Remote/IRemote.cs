using System;
using System.Collections.Generic;

namespace DevNote
{
    public interface IRemote : ISelectableService, IInitializable
    {

        protected Dictionary<RemoteKey, string> Values { get; }



        public string GetString(RemoteKey remoteKey, string defaultValue = "") 
            => Values.ContainsKey(remoteKey) ? Values[remoteKey] : defaultValue;

        public bool GetBool(RemoteKey remoteKey, bool defaultValue = false)
            => Values.ContainsKey(remoteKey) ? Values[remoteKey].FromBinaryToBool() : defaultValue;

        public int GetInt(RemoteKey remoteKey, int defaultValue = 0) 
            => Values.ContainsKey(remoteKey) ? int.Parse(Values[remoteKey]) : defaultValue;

        public float GetFloat(RemoteKey remoteKey, float defaultValue = 0f) 
            => Values.ContainsKey(remoteKey) ? float.Parse(Values[remoteKey]) : defaultValue;



        public bool KeyExists(RemoteKey remoteKey) => Values.ContainsKey(remoteKey);

    }
}


