using System;
using System.Collections.Generic;
using UnityEngine;


namespace DevNote
{
    public static class Extensions
    {

        public static Color SetAlpha(this Color color, float alpha) 
            => new Color(color.r, color.g, color.b, alpha);

        public static T GetRandom<T>(this List<T> list) => list[UnityEngine.Random.Range(0, list.Count)];

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



    }
}

