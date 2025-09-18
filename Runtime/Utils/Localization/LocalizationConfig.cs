using System;
using System.Collections.Generic;
using UnityEngine;


namespace DevNote
{
    [CreateAssetMenu(menuName = "DevNote/Localization", fileName = "Localization")]
    public class LocalizationConfig : LoadableFromTable
    {
        [Serializable] private struct LanguageData
        {
            public Language language;
            public Column column;
        }
        [field: SerializeField] public Language DefaultLanguage { get; private set; }

        [SerializeField] private List<LanguageData> _availableLanguages;
        [field: SerializeField] public List<Translation> Translations { get; private set; }


        public bool LanguageIsAvailable(Language language) 
            => _availableLanguages.Exists(data => data.language == language);


        public override void LoadData(Dictionary<string, Table> tables)
        {
            Translations = new();

            var table = tables["Localization"];

            for (int i = 2; i <= table.Rows; i++)
            {
                string key = table.Get(row: i, column: 0);
                List<(Language, string)> translations = new();

                for (int j = 0; j < table.Columns; j++)
                {
                    var column = (Column)j;

                    int languageIndex = _availableLanguages.FindIndex(data => data.column == column);
                    if (languageIndex != -1)
                    {
                        Language language = _availableLanguages[languageIndex].language;
                        string value = table.Get(row: i, column);
                        translations.Add((language, value));
                    }
                }

                Translations.Add(new Translation(key, translations));
            }
        }
    }
}



