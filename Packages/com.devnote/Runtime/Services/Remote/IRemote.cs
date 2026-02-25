using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{

    public delegate string RemoteDefaultHandler();

    public interface IRemote : ISelectableService, IInitializable
    {
        private static Dictionary<RemoteKey, RemoteDefaultHandler> _defaultHandlers = new();


        protected Dictionary<RemoteKey, string> Values { get; }


        public static void SetDefaultHandler(RemoteKey remoteKey, RemoteDefaultHandler handler)
            => _defaultHandlers[remoteKey] = handler;


        public sealed string GetString(RemoteKey remoteKey)
        {
            if (!_defaultHandlers.ContainsKey(remoteKey))
                throw new Exception($"{Info.Prefix} IRemote: Key \"{remoteKey}\" does not contains default handler! " +
                    $"Use IRemote.SetDefaultHandler() for set default value before using it.");

            return Values.ContainsKey(remoteKey) ? Values[remoteKey] : _defaultHandlers[remoteKey].Invoke();
        }

        public sealed bool GetBool(RemoteKey remoteKey)
        {
            string stringValue = GetString(remoteKey);

            if (stringValue.ToLower() == "true" || stringValue.ToLower() == "false")
                return bool.Parse(stringValue.ToLower());

            else
            {
                string defaultValue = _defaultHandlers[remoteKey].Invoke();
                LogWarning("Bool", remoteKey, stringValue, defaultValue);

                return bool.Parse(defaultValue);
            }
        }

        public int GetInt(RemoteKey remoteKey)
        {
            string stringValue = GetString(remoteKey);

            if (int.TryParse(stringValue, out int result))
                return result;

            else
            {
                string defaultValue = _defaultHandlers[remoteKey].Invoke();
                LogWarning("Int", remoteKey, stringValue, defaultValue);

                return int.Parse(defaultValue);
            }
        }

        public float GetFloat(RemoteKey remoteKey)
        {
            string stringValue = GetString(remoteKey);

            if (float.TryParse(stringValue, out float result))
                return result;

            else
            {
                string defaultValue = _defaultHandlers[remoteKey].Invoke();
                LogWarning("Float", remoteKey, stringValue, defaultValue);

                return float.Parse(defaultValue);
            }
        }

        public T GetEnum<T>(RemoteKey remoteKey) where T : Enum
        {
            string stringValue = GetString(remoteKey);

            if (Enum.TryParse(typeof(T), stringValue, out object result))
                return (T)result;

            else
            {
                string defaultValue = _defaultHandlers[remoteKey].Invoke();
                LogWarning(typeof(T).ToString(), remoteKey, stringValue, defaultValue);

                return (T)Enum.Parse(typeof(T), defaultValue);
            }
        }



        private void LogWarning(string formatName, RemoteKey key, string value, string defaultValue)
            => Debug.LogWarning($"Wrong remote key {formatName} format: {key} = {value}. Now using default value: {defaultValue}");



    }
}


