using System;
using System.Collections.Generic;

namespace DevNote
{
    public interface IRemote : ISelectableService, IInitializable
    {

        protected Dictionary<string, string> Values { get; }



        public string GetString(string remoteKey, string defaultValue = "") 
            => Values.ContainsKey(remoteKey) ? Values[remoteKey] : defaultValue;

        public bool GetBool(string remoteKey, bool defaultValue = false)
            => Values.ContainsKey(remoteKey) ? Values[remoteKey].FromBinaryToBool() : defaultValue;

        public int GetInt(string remoteKey, int defaultValue = 0) 
            => Values.ContainsKey(remoteKey) ? int.Parse(Values[remoteKey]) : defaultValue;

        public float GetFloat(string remoteKey, float defaultValue = 0f) 
            => Values.ContainsKey(remoteKey) ? float.Parse(Values[remoteKey]) : defaultValue;



        public bool KeyExists(string remoteKey) => Values[remoteKey].Contains(remoteKey);

    }
}


