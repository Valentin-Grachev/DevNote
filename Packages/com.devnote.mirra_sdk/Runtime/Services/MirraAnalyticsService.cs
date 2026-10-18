using System.Collections.Generic;
using UnityEngine;
using MirraSdk = MirraGames.SDK.MirraSDK;

namespace DevNote.SDK.MirraSDK
{
    public class MirraAnalyticsService : MonoBehaviour, IAnalytics
    {
        bool IInitializable.Initialized => MirraSdk.IsInitialized;

        bool ISelectableService.IsAvailableForSelection =>
            MirraEnvironmentService.IsAvailableForSelection;

        void IInitializable.Initialize() { }

        void IAnalytics.SendEvent(string eventName, Dictionary<string, object> parameters)
        {
            if (parameters == null || parameters.Count == 0)
                MirraSdk.Analytics.Report(eventName);

            else MirraSdk.Analytics.Report(eventName, parameters);
        }
    }
}
