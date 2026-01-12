using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.Text;


#if UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

namespace DevNote.SDK.YandexMetrika
{
    public class YandexMetrikaAnalyticsService : MonoBehaviour, IAnalytics
    {
        [SerializeField] private bool _useInBuild;
        [SerializeField] private int _counterId;


        bool IInitializable.Initialized => true;

        bool ISelectableService.IsAvailableForSelection
            => !IEnvironment.IsEditor && IEnvironment.PlatformType == PlatformType.WebGL && _useInBuild;

        void IInitializable.Initialize() { }


        void IAnalytics.SendEvent(string eventName, Dictionary<string, object> parameters)
        {
            string jsonParameters = string.Empty;

            if (parameters != null && parameters.Count > 0)
            {
                var keyList = new List<string> { eventName };

                foreach (var parameter in parameters)
                    keyList.Add($"{parameter.Key}={parameter.Value}");

                jsonParameters = BuildNestedJson(keyList);
            }

            else jsonParameters = "{" + $"\"{eventName}\":\"No_params\"" + "}";

            Debug.Log(jsonParameters);
#if UNITY_WEBGL
            _TriggerEvent(_counterId, jsonParameters);
#endif
        }

#if UNITY_WEBGL
        [DllImport("__Internal")] private static extern void _TriggerEvent(int counterId, string eventData);
#endif

        public static string BuildNestedJson(List<string> keys)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < keys.Count - 1; i++)
                builder.Append('{').Append('"').Append(keys[i]).Append('"').Append(':');

            builder.Append('"').Append(keys[^1]).Append('"');

            for (int i = 0; i < keys.Count - 1; i++)
                builder.Append('}');

            return builder.ToString();
        }


    }

}
