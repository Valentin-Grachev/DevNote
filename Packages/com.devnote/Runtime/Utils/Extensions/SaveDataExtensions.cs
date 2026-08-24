using System;
using System.Collections.Generic;
using System.Text;

namespace DevNote
{
    public static class SaveDataExtensions
    {
        public static string ToSaveData<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
        Func<TKey, string> keyHandler, Func<TValue, string> valueHandler)
        {
            var builder = new StringBuilder();

            int i = 0;
            foreach (var keyValue in dictionary)
            {
                if (i != 0) builder.Append(S.S2);

                var keyString = keyHandler(keyValue.Key);
                var valueString = valueHandler(keyValue.Value);

                builder.Append($"{keyString}{S.S1}{valueString}");
                i++;
            }

            return builder.ToString();
        }

        public static Dictionary<TKey, TValue> SaveDataToDictionary<TKey, TValue>(this string data,
            Func<string, TKey> keyParser, Func<string, TValue> valueParser)
        {
            var result = new Dictionary<TKey, TValue>();
            if (string.IsNullOrEmpty(data)) return result;

            var keyValueStrings = data.Split(S.S2);
            foreach (var keyValueString in keyValueStrings)
            {
                var splitData = keyValueString.Split(S.S1);

                var keyString = splitData[0];
                var valueString = splitData[1];

                result.Add(keyParser(keyString), valueParser(valueString));
            }

            return result;
        }


    }
}
