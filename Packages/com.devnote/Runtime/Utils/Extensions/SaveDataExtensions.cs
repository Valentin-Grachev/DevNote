using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace DevNote
{
    public static class SaveDataExtensions
    {

        #region Dictionary

        public static string ToSaveData<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
        Func<TKey, string> keyHandler, Func<TValue, string> valueHandler)
        {
            var builder = new StringBuilder();

            int i = 0;
            foreach (var keyValue in dictionary)
            {
                if (i != 0) builder.Append(S.S4);

                var keyString = keyHandler(keyValue.Key);
                var valueString = valueHandler(keyValue.Value);

                builder.Append($"{keyString}{S.S3}{valueString}");
                i++;
            }

            return builder.ToString();
        }

        public static Dictionary<TKey, TValue> SaveDataToDictionary<TKey, TValue>(this string data,
            Func<string, TKey> keyParser, Func<string, TValue> valueParser)
        {
            var result = new Dictionary<TKey, TValue>();
            if (string.IsNullOrEmpty(data)) return result;

            var keyValueStrings = data.Split(S.S4);
            foreach (var keyValueString in keyValueStrings)
            {
                var splitData = keyValueString.Split(S.S3);

                var keyString = splitData[0];
                var valueString = splitData[1];

                result.Add(keyParser(keyString), valueParser(valueString));
            }

            return result;
        }

        #endregion

        #region TimeSpan

        public static string ToSaveData(this TimeSpan time) 
            => time.Ticks.ToString(CultureInfo.InvariantCulture);

        public static TimeSpan SaveDataToTimeSpan(this string data) 
            => TimeSpan.FromTicks(long.Parse(data, CultureInfo.InvariantCulture));

        #endregion

        #region DateTime


        public static string ToDataString(this DateTime dateTime)
            => dateTime.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.GetCultureInfo("ru-RU"));

        public static DateTime SaveDataToDateTime(this string data)
        {
            bool success = DateTime.TryParseExact(data, "dd.MM.yyyy HH:mm:ss",
                CultureInfo.GetCultureInfo("ru-RU"), DateTimeStyles.None, out DateTime dateTime);

            if (success) return dateTime;
            else
            {
                Debug.LogError($"[DATA ERROR] String \"{data}\" has incorrect DateTime format! Now using default value = DateTime.MinValue.");
                return DateTime.MinValue;
            }
        }

        #endregion

        #region List

        public static string ToSaveData<T>(this List<T> list, Func<T, string> handler)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < list.Count; i++)
            {
                if (i != 0) builder.Append(S.S4);
                builder.Append(handler(list[i]));
            }

            return builder.ToString();
        }

        public static List<T> SaveDataToList<T>(this string data, Func<string, T> parser)
        {
            var result = new List<T>();

            if (string.IsNullOrEmpty(data)) return result;

            var splitData = data.Split(S.S4);

            foreach (var stringValue in splitData)
                result.Add(parser(stringValue));

            return result;
        }

        #endregion

    }
}

