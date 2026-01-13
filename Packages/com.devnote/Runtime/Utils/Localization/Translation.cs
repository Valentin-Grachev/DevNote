using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    public enum Language { RU, EN, TR }

    [Serializable] public struct Translation
    {
        [Serializable] private struct TranslationValue
        {
            public Language language;
            public string value;
        }

        public string key;
        [SerializeField] private List<TranslationValue> _values;


        public Translation(string key, List<(Language, string)> translations)
        {
            this.key = key;
            _values = new List<TranslationValue>();

            foreach (var translation in translations)
                _values.Add(new TranslationValue { language = translation.Item1, value = translation.Item2 });
        }


        public string GetTranslation(Language language) 
            => _values.Find(value => value.language == language).value;

    }

}

