using System;
using System.Collections.Generic;
using UnityEngine;
using MirraSdk = MirraGames.SDK.MirraSDK;

namespace DevNote.SDK.MirraSDK
{
    public class MirraReviewService : MonoBehaviour, IReview
    {
        [SerializeField] private List<KeyValue<MirraGames.SDK.Common.PlatformType, bool>> _supportReviewPlatforms;


        bool IInitializable.Initialized => MirraSdk.IsInitialized;

        bool ISelectableService.IsAvailableForSelection => MirraEnvironmentService.IsAvailableForSelection;

        bool IReview.ReviewIsAvailable => _supportReviewPlatforms.Exists(MirraSdk.Platform.Current) ?
           _supportReviewPlatforms.Get(MirraSdk.Platform.Current) : false;

        void IInitializable.Initialize() { }

        void IReview.Rate(Action onGameRated, Action onRejected)
        {
            MirraSdk.Platform.RateGame();
            IReview.GameRated(onGameRated);
        }
    }
}
