using System.Collections.Generic;
using UnityEngine;

namespace DevNote.Services.Test
{
    public class TestAnalyticsService : MonoBehaviour, IAnalytics
    {
        bool IProjectInitializable.Initialized => true;

        bool ISelectableService.Available => true;

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


