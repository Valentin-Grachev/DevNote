using UnityEngine;

namespace DevNote.Extra
{
    [CreateAssetMenu(menuName = "DevNote/Extra/Shine Image Animation", fileName = "ShineImage")]
    public class ShineImageAnimationPreset : ScriptableObject
    {
        [field: SerializeField] public Vector2 FromToAlpha { get; private set; }
        [field: SerializeField] public float LoopDuration { get; private set; } = 1f;




    }
}
