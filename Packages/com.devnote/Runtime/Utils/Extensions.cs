using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace DevNote
{
    public static class Extensions
    {

        public static Color SetAlpha(this Color color, float alpha) 
            => new Color(color.r, color.g, color.b, alpha);


        public static Vector3 SetX(this Vector3 vector, float value) => new Vector3(value, vector.y, vector.z);
        public static Vector3 SetY(this Vector3 vector, float value) => new Vector3(vector.x, value, vector.z);
        public static Vector3 SetZ(this Vector3 vector, float value) => new Vector3(vector.x, vector.y, value);
        public static Vector2 SetX(this Vector2 vector, float value) => new Vector2(value, vector.y);
        public static Vector2 SetY(this Vector2 vector, float value) => new Vector2(vector.x, value);


        public static bool IsPrefab(this GameObject gameObject) 
            => gameObject.scene == null || gameObject.scene.IsValid() == false;


        public static void DestroyChildObjects(this Transform transform)
        {
            foreach (Transform child in transform)
                UnityEngine.Object.Destroy(child.gameObject);
        }


        public static string ToBinaryString(this bool value) => value ? "1" : "0";

        public static bool FromBinaryToBool(this string value) => value switch
        {
            "0" => false,
            "1" => true,
            _ => throw new Exception($"Wrong convertion to bool: {value}"),
        };


        public static void Add<T1, T2>(this Dictionary<T1, T2> toDictionary, Dictionary<T1, T2> dictionary)
        {
            foreach (var item in dictionary)
                toDictionary.Add(item.Key, item.Value);
        }


        public static T ToEnum<T>(this string enumString) where T : Enum 
            => (T)Enum.Parse(typeof(T), enumString);



        public static T GetRandom<T>(this List<T> list) => list[UnityEngine.Random.Range(0, list.Count)];

        public static void Shuffle<T>(this List<T> list)
        {
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static T FindOrException<T>(this List<T> list, Predicate<T> predicate)
        {
            int index = list.FindIndex(predicate);
            if (index == -1) throw new Exception("List doesn't contain the desired value!");
            return list[index];
        }

        public static bool TryFind<T>(this List<T> list, Predicate<T> predicate, out T value)
        {
            int index = list.FindIndex(predicate);
            if (index == -1)
            {
                value = default;
                return false;
            }
            else
            {
                value = list[index];
                return true;
            }
        }


        public static string ToSaveData<T>(this List<T> list, Func<T, string> converter)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < list.Count; i++)
            {
                if (i != 0) builder.Append(S.S4);
                builder.Append(converter(list[i]));
            }

            return builder.ToString();
        }

        public static List<T> SaveDataToList<T>(this string data, Func<string, T> converter)
        {
            var result = new List<T>();

            if (string.IsNullOrEmpty(data)) return result;

            var splitData = data.Split(S.S4);

            foreach (var stringValue in splitData)
                result.Add(converter(stringValue));

            return result;
        }


        public static float ToFloat(this string data)
        {
            if (float.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out float value))
                return value;

            else
            {
                Debug.LogError($"[DATA ERROR] String \"{data}\" has incorrect Float format! Now using default value = 0f.");
                return 0f;
            }
        }

        public static string ToDataString(this float value) => value.ToString(CultureInfo.InvariantCulture);

        public static string ToDataString(this DateTime dateTime)
            => dateTime.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.GetCultureInfo("ru-RU"));

        public static DateTime ToDateTime(this string data)
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

        public static TimeSpan ToTimeSpan(this string data) 
            => TimeSpan.FromTicks(long.Parse(data, CultureInfo.InvariantCulture));


        public static string ToDataString(this TimeSpan timeSpan)
            => timeSpan.Ticks.ToString(CultureInfo.InvariantCulture);


        public static void AddUnique<T>(this List<T> list, T item)
        {
            if (!list.Contains(item)) list.Add(item);
        }


        public static async UniTask<T> LoadAssetWithKey<T>(this AssetReferenceT<T> assetReference) where T : UnityEngine.Object
        {
            string key = (string)assetReference.RuntimeKey;
            return await Addressables.LoadAssetAsync<T>(key);
        }

    }
}

