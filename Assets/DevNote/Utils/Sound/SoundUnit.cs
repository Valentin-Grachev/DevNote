using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;


namespace DevNote
{
    [CreateAssetMenu(menuName = "DevNote/" + nameof(SoundUnit), fileName = nameof(SoundUnit))]
    public class SoundUnit : ScriptableObject
    {
        public enum PlayType { Simple, Loop, OneShot }


        [SerializeField] private Sound.Channel _channel; public Sound.Channel channel => _channel;
        [SerializeField] private PlayType _playType; public PlayType playType => _playType;


        
        [SerializeField, HideIf(nameof(_useRandomAudioClip))] private AudioClip _audioClip;
        [SerializeField, ShowIf(nameof(_useRandomAudioClip))] private List<AudioClip> _randomAudioClips;
        
        [SerializeField, HideIf(nameof(_useRandomVolume))] [Range(0f, 1f)] private float _volume = 1f;
        [SerializeField, MinMaxSlider(0f, 1f), ShowIf(nameof(_useRandomVolume))] private Vector2 _randomVolume;

        [SerializeField, Range(-3f, 3f), HideIf(nameof(_useRandomPitch))] private float _pitch = 1f;
        [SerializeField, MinMaxSlider(-3f, 3f), ShowIf(nameof(_useRandomPitch))] private Vector2 _randomPitch;

        [Space(10)]
        [SerializeField] private bool _useRandomAudioClip;
        [SerializeField] private bool _useRandomVolume;
        [SerializeField] private bool _useRandomPitch;



        public AudioClip audioClip => _useRandomAudioClip ? 
            _randomAudioClips[Random.Range(0, _randomAudioClips.Count)] : _audioClip;

        public float volume => _useRandomVolume ?
            Random.Range(_randomVolume.x, _randomVolume.y) : _volume;

        public float pitch => _useRandomPitch ?
            Random.Range(_randomPitch.x, _randomPitch.y) : _pitch;


        public void Play() => Sound.Play(this);

        /*
        private void OnValidate()
        {
            if (_audioClip != null) _audioClipName = _audioClip.name;
            _audioClipNames = new List<string>() { "Readf", "sfaf" };
        }
        */


    }




}


