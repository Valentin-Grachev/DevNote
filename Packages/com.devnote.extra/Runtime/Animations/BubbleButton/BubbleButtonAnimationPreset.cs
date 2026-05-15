using UnityEngine;

namespace DevNote.Extra
{
    [CreateAssetMenu(menuName = "DevNote/Extra/Bubble Button Animation", fileName = "BubbleButton")]
    public class BubbleButtonAnimationPreset : ScriptableObject
    {
        [field: SerializeField] public SoundUnit ClickSound { get; private set; }
        [field: SerializeField] public SoundUnit PointerEnterSound { get; private set; }
        [field: SerializeField] public SoundUnit PointerExitSound { get; private set; }


        [field: Space, SerializeField] public float HighlightScale { get; private set; }
        [field: SerializeField] public float HighlightDuration { get; private set; }

        [field: Space, SerializeField] public float ClickScale { get; private set; }
        [field: SerializeField] public float ClickDuration { get; private set; }


    }
}
