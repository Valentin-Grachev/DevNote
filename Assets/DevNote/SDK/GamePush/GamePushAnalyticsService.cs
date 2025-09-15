using System.Collections.Generic;
using System.Linq;
using GamePush;
using UnityEngine;

namespace DevNote.Services.GamePush
{
    public class GamePushAnalyticsService : MonoBehaviour, IAnalytics
    {
        public bool IsAvailableForSelection => GamePushEnvironmentService.ServicesIsAvailable;


        public bool Initialized => GP_Init.isReady;
        public void Initialize() { }

        public void SendEvent(string eventName, Dictionary<string, object> parameters = null)
        {
            object firstParameter = parameters.ToList()[0].Value;

            if (firstParameter is int)
                GP_Analytics.Goal(eventName, (int)firstParameter);

            else GP_Analytics.Goal(eventName, firstParameter.ToString());
        }
    }

}

