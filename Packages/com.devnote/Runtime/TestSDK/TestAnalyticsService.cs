using System.Collections.Generic;
using UnityEngine;

namespace DevNote.SDK.Test
{
    public class TestAnalyticsService : MonoBehaviour, IAnalytics
    {
        bool IInitializable.Initialized => true;

        bool ISelectableService.IsAvailableForSelection => true;

        public void Initialize() { }

        void IAnalytics.SendEvent(string eventKey, Dictionary<string, object> parameters)
        {
            string parametersDataString = string.Empty;

            if (parameters != null)
            {
                foreach (var parameter in parameters)
                    parametersDataString += $"({parameter.Key}: {parameter.Value}) ";
            }

            Debug.Log($"{Info.Prefix} Send event \"{eventKey}\"; {parametersDataString}");
        }
    }
}


