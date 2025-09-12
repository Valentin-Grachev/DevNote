using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace DevNote
{
    public static class Extensions
    {
        
        public static Color SetAlpha(this Color color, float alpha) 
            => new Color(color.r, color.g, color.b, alpha);

        public static T GetRandom<T>(this List<T> list) => list[Random.Range(0, list.Count)];

        public static Vector3 SetX(this Vector3 vector, float value) => new Vector3(value, vector.y, vector.z);
        public static Vector3 SetY(this Vector3 vector, float value) => new Vector3(vector.x, value, vector.z);
        public static Vector3 SetZ(this Vector3 vector, float value) => new Vector3(vector.x, vector.y, value);
        public static Vector2 SetX(this Vector2 vector, float value) => new Vector2(value, vector.y);
        public static Vector2 SetY(this Vector2 vector, float value) => new Vector2(vector.x, value);


        public static T Attach<T>(this T tween, GameObject gameObject) where T : Tween
        {
            tween.SetLink(gameObject, LinkBehaviour.KillOnDisable);
            return tween;
        }

        public static bool IsPrefab(this GameObject gameObject) 
            => gameObject.scene == null || gameObject.scene.IsValid() == false;




    }
}

