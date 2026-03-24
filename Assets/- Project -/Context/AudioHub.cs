using DevNote;
using NaughtyAttributes;
using UnityEngine;


[CreateAssetMenu(menuName = "Configs/Audio Hub", fileName = "AudioHub")]
public class AudioHub : ScriptableObject
{

    [field: Foldout("MAIN"), Expandable, SerializeField] public SoundUnit Show { get; private set; }




}
