using System;

namespace DevNote
{
    public interface IRemote : ISelectableService, IInitializable
    {
        public string GetString(string remoteKey);


        public bool GetBool(string remoteKey) => GetString(remoteKey).FromBinaryToBool();
        public int GetInt(string remoteKey) => int.Parse(GetString(remoteKey));
        public float GetFloat(string remoteKey) => float.Parse(GetString(remoteKey));



        protected static Exception UndefinedKeyException(Type serviceType, string remoteKey) 
            => new Exception($"[{serviceType.Name}] Undefined remote key: {remoteKey}");

    }
}


