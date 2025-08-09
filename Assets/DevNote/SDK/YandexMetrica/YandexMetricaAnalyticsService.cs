using System.Collections.Generic;
using System.Globalization;
using NaughtyAttributes;
using UnityEngine;

#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

namespace DevNote.Services.YandexMetrica
{
    public class YandexMetricaAnalyticsService : MonoBehaviour, IAnalytics
    {
        [Expandable] [SerializeField] private YandexMetricaConfig _config;


        bool ISelectableService.Available => !IEnvironment.IsEditor && IEnvironment.PlatformType == PlatformType.WebGL;


        bool IInitializable.Initialized => true;
        void IInitializable.Initialize() { }


        void IAnalytics.SendEvent(string eventName, Dictionary<string, object> parameters)
        {
            var convertedParameters = new Dictionary<string, string>();

            foreach (var parameter in parameters)
                convertedParameters.Add(parameter.Key, parameter.Value.ToString());

            string jsonParameters = string.Empty;

            if (parameters != null && parameters.Count > 0)
                jsonParameters = ToJson(convertedParameters);

#if UNITY_WEBGL
            _SendEvent(_config.YandexMetricaCounterId, eventName, jsonParameters);
#endif
        }

#if UNITY_WEBGL
        [DllImport("__Internal")] private static extern void _SendEvent(int counterId, string eventName, string eventData);
#endif


        private string ToJson(IDictionary<string, string> dictionary)
        {
            var jsonString = "{";
            var kvpCount = 0;

            foreach (var kvp in dictionary)
            {
                if (string.IsNullOrEmpty(kvp.Key) || string.IsNullOrEmpty(kvp.Value)) continue;
                jsonString += $"\"{kvp.Key}\":{GetValueString(kvp.Value)},";
                kvpCount++;
            }

            if (kvpCount == 0) return string.Empty;

            if (dictionary.Count > 0)
                jsonString = jsonString.Remove(jsonString.Length - 1);

            jsonString += "}";

            return jsonString;
        }

        private string GetValueString(string value)
        {
            if (int.TryParse(value, out var intValue))
                return intValue.ToString();

            if (float.TryParse(value, out var floatValue))
                return floatValue.ToString(CultureInfo.InvariantCulture);

            if (bool.TryParse(value, out var boolValue))
                return boolValue.ToString().ToLower();

            value = value.Replace("\\", "\\\\").Replace("\"", "\\\"");
            return $"\"{value}\"";
        }
    }

}
