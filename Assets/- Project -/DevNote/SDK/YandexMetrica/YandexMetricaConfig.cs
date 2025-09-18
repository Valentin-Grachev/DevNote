using UnityEngine;

namespace DevNote.Services.YandexMetrica
{
    public class YandexMetricaConfig : ScriptableObject
    {
        [field: SerializeField] public bool IncludeInBuild { get; private set; }
        [field: SerializeField] public int YandexMetricaCounterId { get; private set; }



    }
}



