using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace DevNote
{
    [CreateAssetMenu(menuName = "DevNote/" + nameof(SoundUnit), fileName = nameof(SoundUnit))]
    public class SoundUnit : ScriptableObject
    {
        public enum PlayType { Simple, Loop, OneShot }

        [Space(10), SerializeField, Label("▶ PLAY")] private bool _clickToPlay;

        [Space(20)]
        [SerializeField] private Sound.Channel _channel; public Sound.Channel channel => _channel;
        [SerializeField] private PlayType _playType; public PlayType playType => _playType;

        [Space(20)]
        [SerializeField] private bool _useAddressables; private bool NotUseAddressables => !_useAddressables;
        [SerializeField] private bool _useRandomAudioClip; private bool NotUseRandomAudioClip => !_useRandomAudioClip;

        [SerializeField, ShowIf(EConditionOperator.And, nameof(NotUseAddressables), nameof(NotUseRandomAudioClip))] 
        private AudioClip _audioClip;

        [SerializeField, ShowIf(EConditionOperator.And, nameof(NotUseAddressables), nameof(_useRandomAudioClip))]
        private List<AudioClip> _randomAudioClips;

        [SerializeField, ShowIf(EConditionOperator.And, nameof(_useAddressables), nameof(NotUseRandomAudioClip))]
        private AssetReferenceT<AudioClip> _audioClipReference;

        [SerializeField, ShowIf(EConditionOperator.And, nameof(_useAddressables), nameof(_useRandomAudioClip))]
        private List<AssetReferenceT<AudioClip>> _randomAudioClipReferences;

        [Space(20)]
        [SerializeField] private bool _useRandomVolume;
        [SerializeField, HideIf(nameof(_useRandomVolume))] [Range(0f, 1f)] private float _volume = 1f;
        [SerializeField, MinMaxSlider(0f, 1f), ShowIf(nameof(_useRandomVolume))] private Vector2 _randomVolume;
        [Space(10)]
        [SerializeField] private bool _useRandomPitch;
        [SerializeField, Range(-3f, 3f), HideIf(nameof(_useRandomPitch))] private float _pitch = 1f;
        [SerializeField, MinMaxSlider(-3f, 3f), ShowIf(nameof(_useRandomPitch))] private Vector2 _randomPitch;

        private Dictionary<AssetReferenceT<AudioClip>, AsyncOperationHandle<AudioClip>> _cashedHandlers = new();

        
        public async UniTask<AudioClip> GetAudioClip()
        {
            if (_useAddressables)
            {
                var audioReference = _useRandomAudioClip ? 
                    _randomAudioClipReferences.GetRandom() : _audioClipReference;

                if (_cashedHandlers.ContainsKey(audioReference))
                    return _cashedHandlers[audioReference].Result;

                else
                {
                    var handler = audioReference.LoadAssetAsync();
                    _cashedHandlers.Add(audioReference, handler);

                    return await handler.ToUniTask();
                }
            }
            else
            {
                var audioClip = _useRandomAudioClip ? _randomAudioClips.GetRandom() : _audioClip;
                return audioClip;
            }
        }

        public float Volume => _useRandomVolume ?
            Random.Range(_randomVolume.x, _randomVolume.y) : _volume;

        public float Pitch => _useRandomPitch ?
            Random.Range(_randomPitch.x, _randomPitch.y) : _pitch;


        public void Play() => Sound.Play(this).Forget();

        public async UniTask<AudioSource> PlayAsync() => await Sound.Play(this);


        private void OnValidate()
        {
            if (_clickToPlay && Application.isPlaying) Play();

            _clickToPlay = false;

            if (_useAddressables)
            {
                _audioClip = null;
                _randomAudioClips = null;
            }
            else
            {
                _audioClipReference = null;
                _randomAudioClipReferences = null;
            }
        }



    }




}


