using UnityEngine;

namespace DevNote.Extra
{
    [CreateAssetMenu(menuName = "DevNote/Extra/Rotation Animation", fileName = "Rotation")]
    public class RotationAnimationPreset : ScriptableObject
    {
        [field: SerializeField] public float LoopDuration { get; private set; } = 1f;
        
    }

}
