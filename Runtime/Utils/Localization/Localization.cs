using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace DevNote
{

    public class Localization : MonoBehaviour, IInitializable
    {
        public static event Action OnLanguageChanged;
        public static Language CurrentLanguage { get; private set; }
        public static bool Initialized => _instance != null && _instance._initialized;

        private static Localization _instance;


        [SerializeField] private GoogleTables _googleTables;

        private Dictionary<string, Translation> _tranlationDictionary = new();
        private LocalizationConfig _config;
        private bool _initialized = false;

        private readonly Holder<IEnvironment> environment = new();

        private const string LANGUAGE_SAVE_KEY = "language";


        bool IInitializable.Initialized => _initialized;

        async void IInitializable.Initialize()
        {
            _instance = this;
            _config = Resources.Load<LocalizationConfig>("Localization");

            await UniTask.WaitUntil(() => (_googleTables as IInitializable).Initialized);

            foreach (var translation in _config.Translations)
            {
                if (translation.key != string.Empty)
                    _tranlationDictionary.Add(translation.key, translation);
            }

            Language language = Language.EN;

            if (PlayerPrefs.HasKey(LANGUAGE_SAVE_KEY))
                language = (Language)Enum.Parse(typeof(Language), PlayerPrefs.GetString(LANGUAGE_SAVE_KEY));

            else
            {
                await UniTask.WaitUntil(() => environment.Item.Initialized);
                language = environment.Item.DeviceLanguage;
            }

            InitializeLanguage(language);

            _initialized = true;
        }

        private static void InitializeLanguage(Language language)
        {
            CurrentLanguage = _instance._config.LanguageIsAvailable(language) ?
                language : _instance._config.DefaultLanguage;
        }

        public static void SetLanguage(Language language)
        {
            CurrentLanguage = _instance._config.LanguageIsAvailable(language) ?
                language : _instance._config.DefaultLanguage;

            PlayerPrefs.SetString(LANGUAGE_SAVE_KEY, language.ToString());

            OnLanguageChanged?.Invoke();
        }

        public static string GetLocalizedText(string key)
        {
            if (_instance._tranlationDictionary.ContainsKey(key) == false)
            {
                Debug.LogWarning($"{Info.Prefix} Translation key \"{key}\" does'nt exist!");
                return key;
            }

            return _instance._tranlationDictionary[key].GetTranslation(CurrentLanguage);
        }


    }


}


