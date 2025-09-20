using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DevNote.SDK.YandexGames
{
    public class YandexGamesEnvironmentService : MonoBehaviour, IEnvironment
    {
        private bool _initialized = false;
        private Language _definedLanguage;

        bool ISelectableService.IsAvailableForSelection => YG_Sdk.ServicesIsSupported;

        bool IInitializable.Initialized => _initialized;
        async void IInitializable.Initialize()
        {
            IEnvironment.StartGameTime = DateTime.Now;

            var sdkPrefab = Resources.Load<YG_Sdk>("YandexGames");

            var sdkObject = Instantiate(sdkPrefab, parent: null);
            sdkObject.name = "YandexGames";

            DontDestroyOnLoad(sdkObject);

            await UniTask.WaitUntil(() => YG_Sdk.available);
            
            _definedLanguage = YG_Sdk.GetLanguage() switch
            {
                "ru" => Language.RU,
                "en" => Language.EN,
                "tr" => Language.TR,
                _ => Language.EN
            };

            _initialized = true;
        }


        Language IEnvironment.DeviceLanguage => _definedLanguage;

        DeviceType IEnvironment.DeviceType => YG_Sdk.GetDeviceType();

        void IEnvironment.GameReady() => YG_GameReady.GameReady();


        
    }

}

