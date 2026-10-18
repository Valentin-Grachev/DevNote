using System.Collections.Generic;
using UnityEngine;
using MirraPlatformType = MirraGames.SDK.Common.PlatformType;
using MirraSdk = MirraGames.SDK.MirraSDK;

namespace DevNote.SDK.MirraSDK
{
    public class MirraFeatureService : MonoBehaviour, IFeature
    {
        private readonly List<MirraPlatformType> _startMenuPlatforms = new()
        {
            MirraPlatformType.OK,
            MirraPlatformType.VK,
        };


        bool IInitializable.Initialized => MirraSdk.IsInitialized;

        bool IFeature.IsStartMenu => _startMenuPlatforms.Contains(MirraSdk.Platform.Current);

        bool ISelectableService.IsAvailableForSelection => 
            MirraEnvironmentService.IsAvailableForSelection;

        void IInitializable.Initialize() { }
    }
}
