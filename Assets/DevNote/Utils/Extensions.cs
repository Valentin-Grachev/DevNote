using System.Collections.Generic;
using UnityEngine;


namespace DevNote
{
    public static class Extensions
    {
        
        public static Color SetAlpha(this Color color, float alpha) 
            => new Color(color.r, color.g, color.b, alpha);


        public static T GetRandom<T>(this List<T> list) => list[Random.Range(0, list.Count)];



    }
}

