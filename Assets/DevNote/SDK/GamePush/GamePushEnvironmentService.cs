using System;
using GamePush;
using UnityEngine;

namespace DevNote.Services.GamePush
{
    public class GamePushEnvironmentService : MonoBehaviour, IEnvironment
    {

        public static bool ServicesIsAvailable => 
            IEnvironment.PlatformType == PlatformType.WebGL &&
            IEnvironment.EnvironmentType == EnvironmentType.GamePush;


        bool ISelectableService.Available => ServicesIsAvailable;


        Language IEnvironment.CurrentLanguage => GP_Language.CurrentISO() switch
        {
            "ru" => Language.RU,
            "en" => Language.EN,
            "tr" => Language.TR,

            _ => Language.EN,
        };

        DeviceType IEnvironment.DeviceType => GP_Device.IsMobile() ? DeviceType.Mobile : DeviceType.Desktop;


        bool IInitializable.Initialized => GP_Init.isReady;

        DateTime IEnvironment.ServerTime => GP_Server.Time();

        void IInitializable.Initialize() {}


        void IEnvironment.GameReady() => GP_Game.GameReady();

        
    }
}


