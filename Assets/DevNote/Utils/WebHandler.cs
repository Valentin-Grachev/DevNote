using System;
using UnityEngine;

namespace DevNote
{
    public class WebHandler : MonoBehaviour
    {
        public static Action OnPageBeforeUnload;
        public static Action OnPageHidden;


        private void Awake()
        {
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }

        public void JS_OnPageBeforeUnload() => OnPageBeforeUnload?.Invoke();

        public void JS_OnPageHidden() => OnPageHidden?.Invoke();



    }
}



