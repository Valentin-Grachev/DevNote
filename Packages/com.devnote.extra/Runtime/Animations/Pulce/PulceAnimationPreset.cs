using UnityEngine;

namespace DevNote.Extra
{
    [CreateAssetMenu(menuName = "DevNote/Extra/Pulce Animation", fileName = "Pulce")]
    public class PulceAnimationPreset : ScriptableObject
    {
        [field: SerializeField] public float LoopDuration { get; private set; } = 1f;
        [field: SerializeField] public Vector2 FromToScale { get; private set; }
    }
}
