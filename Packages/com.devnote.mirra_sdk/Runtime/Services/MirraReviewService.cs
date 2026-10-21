using System;
using System.Collections.Generic;
using UnityEngine;
using MirraSdk = MirraGames.SDK.MirraSDK;

namespace DevNote.SDK.MirraSDK
{
    public class MirraReviewService : MonoBehaviour, IReview
    {
        [SerializeField] private bool _editorSupportReview;
        [SerializeField] private List<KeyValue<MirraGames.SDK.Common.PlatformType, bool>> _supportReviewPlatforms;


        bool IInitializable.Initialized => MirraSdk.IsInitialized;

        bool ISelectableService.IsAvailableForSelection => MirraEnvironmentService.IsAvailableForSelection;

        bool IReview.ReviewIsAvailable
        {
            get
            {
                if (IEnvironment.IsEditor)
                    return _editorSupportReview;

                else if (_supportReviewPlatforms.Exists(MirraSdk.Platform.Current))
                    return _supportReviewPlatforms.Get(MirraSdk.Platform.Current);

                return false;
            }
        }

        void IInitializable.Initialize() { }

        void IReview.Rate(Action onGameRated, Action onRejected)
        {
            MirraSdk.Platform.RateGame();
            IReview.GameRated(onGameRated);
        }
    }
}
