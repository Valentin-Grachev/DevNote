using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace DevNote
{
    [CreateAssetMenu(menuName = "DevNote/" + nameof(SoundUnit), fileName = nameof(SoundUnit))]
    public class SoundUnit : ScriptableObject
    {
        public enum PlayType { Simple, Loop, OneShot }

        private enum AssemblyType { BuildIn, Addressable }


        [Space(10), SerializeField, Label("▶ PLAY")] private bool _clickToPlay;

        [Space(15)]
        [SerializeField] private Sound.Channel _channel; public Sound.Channel channel => _channel;
        [SerializeField] private PlayType _playType; public PlayType playType => _playType;
        

        


        [Space(15)]
        [SerializeField] private AssemblyType _assemblyType = AssemblyType.BuildIn;

        [SerializeField, ShowIf(nameof(NotUseAddressables))] private AudioClip _audioClip;

        [SerializeField, ShowIf(nameof(UseAddressables))] private AssetReferenceT<AudioClip> _audioClipReference;


        [Space(15)]
        [SerializeField] private bool _useRandomVolume;
        [SerializeField, HideIf(nameof(_useRandomVolume))] [Range(0f, 1f)] private float _volume = 1f;
        [SerializeField, MinMaxSlider(0f, 1f), ShowIf(nameof(_useRandomVolume))] private Vector2 _randomVolume;
        [Space(15)]
        [SerializeField] private bool _useRandomPitch;
        [SerializeField, Range(-3f, 3f), HideIf(nameof(_useRandomPitch))] private float _pitch = 1f;
        [SerializeField, MinMaxSlider(-3f, 3f), ShowIf(nameof(_useRandomPitch))] private Vector2 _randomPitch;

        private Dictionary<AssetReferenceT<AudioClip>, AsyncOperationHandle<AudioClip>> _cashedHandlers = new();

        private bool NotUseAddressables => _assemblyType == AssemblyType.BuildIn;
        private bool UseAddressables => _assemblyType == AssemblyType.Addressable;


        public async UniTask<AudioClip> GetAudioClip()
        {
            if (UseAddressables)
            {

                if (_cashedHandlers.ContainsKey(_audioClipReference))
                    return _cashedHandlers[_audioClipReference].Result;

                else
                {
                    var handler = _audioClipReference.LoadAssetAsync();
                    _cashedHandlers.Add(_audioClipReference, handler);

                    return await handler.ToUniTask();
                }
            }
            else return _audioClip;

        }

        public float Volume => _useRandomVolume ?
            Random.Range(_randomVolume.x, _randomVolume.y) : _volume;

        public float Pitch => _useRandomPitch ?
            Random.Range(_randomPitch.x, _randomPitch.y) : _pitch;


        public void Play() => Sound.Play(this).Forget();

        public async UniTask<AudioSource> PlayAsync() => await Sound.Play(this);


        private void OnValidate()
        {
            // <-- Play preview sound -->
            if (Application.isPlaying)
            {
                if (_clickToPlay) Play();
            }

            else // <-- Handle Addressables/Buildin -->
            {
                if (UseAddressables)
                {
                    if (_audioClip != null)
                    {
                        var path = AssetDatabase.GetAssetPath(_audioClip);
                        var reference = Utils.MakeAssetAsAddressable<AudioClip>(path, "Sounds");
                        _audioClipReference = reference;
                    }

                    _audioClip = null;
                }
                else
                {
                    if (_audioClipReference != null && !string.IsNullOrWhiteSpace(_audioClipReference.AssetGUID))
                    {
                        var guid = _audioClipReference.AssetGUID;
                        var asset = Utils.RemoveAssetFromAddressables<AudioClip>(guid);
                        _audioClip = asset;
                    }

                    _audioClipReference = null;
                }
            }

            _clickToPlay = false;
        }


    }




}


