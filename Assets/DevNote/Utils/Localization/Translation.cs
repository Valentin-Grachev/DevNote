using System.Collections.Generic;

namespace DevNote
{
    public enum Language { RU, EN, TR }


    [System.Serializable]
    public struct Translation
    {
        public string key;
        public string ru, en, tr;

        public string GetTranslation(Language language) => language switch
        { 
            Language.RU => ru,
            Language.EN => en,
            Language.TR => tr,

            _ => throw new System.Exception($"{Info.Prefix} Wrong language: {language}. Please add this language to Translation struct")
        };


        public readonly static List<TableKey> TranslationTableKeys = new List<TableKey>
        {
            TableKey.Localization,
        };


    }

}

