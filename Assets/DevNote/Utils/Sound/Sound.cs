using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace DevNote
{

    public class Sound : MonoBehaviour, IProjectInitializable
    {
        public enum Channel { Music, SFX }

        private bool _initialized = false; public static bool Initialized => _instance != null && _instance._initialized;

        private static bool UseWebAudio => false;// IEnvironment.PlatformType == PlatformType.WebGL;



        public class Settings
        {
            public static bool MusicEnabled
            {
                get => Convert.ToBoolean(PlayerPrefs.GetInt("Music", 1));
                set
                {
                    float volume = value ? 0f : -80f;
                    _instance._audioMixer.SetFloat("musicVolume", volume);
                    PlayerPrefs.SetInt("Music", value ? 1 : 0);
                }
            }

            public static bool SfxEnabled
            {
                get => Convert.ToBoolean(PlayerPrefs.GetInt("Sound", 1));
                set
                {
                    float volume = value ? 0f : -80f;
                    _instance._audioMixer.SetFloat("sfxVolume", volume);
                    PlayerPrefs.SetInt("Sound", value ? 1 : 0);
                }
            }

            public static void Apply()
            {
                SfxEnabled = SfxEnabled;
                MusicEnabled = MusicEnabled;
            }
        }

        private static Sound _instance;


        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private AudioPool _sfxAudioPool;
        [SerializeField] private AudioWebCash _audioWebCash;

        private AudioMixerGroup _sfxGroup;
        private AudioSource _musicAudioSource;

        bool IProjectInitializable.Initialized => _initialized;

        async void IProjectInitializable.Initialize()
        {
            _instance = this;

            _sfxGroup = _audioMixer.FindMatchingGroups("SFX")[0];

            _musicAudioSource = _sfxAudioPool.GetAudioSource();
            _musicAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Music")[0];

            Settings.Apply();

            if (UseWebAudio)
            {
                IProjectInitializable initializable = _audioWebCash;
                initializable.Initialize();
                await UniTask.WaitUntil(() => initializable.Initialized);
            }

            _initialized = true;
        }



        public static AudioSource Play(string soundKey)
        {
            var soundUnit = Resources.Load<SoundUnit>($"Sounds/{soundKey}");

            AudioSource audioSource = soundUnit.channel == Channel.Music ? 
                _instance._musicAudioSource : _instance._sfxAudioPool.GetAudioSource();

            audioSource.clip = UseWebAudio ?
                _instance._audioWebCash.GetClip(soundUnit.audioClip.name) : soundUnit.audioClip;

            if (soundUnit.channel == Channel.SFX)
            audioSource.outputAudioMixerGroup = _instance._sfxGroup;

            audioSource.volume = soundUnit.volume;
            audioSource.loop = soundUnit.playType == SoundUnit.PlayType.Loop;
            audioSource.pitch = soundUnit.pitch;

            if (soundUnit.playType == SoundUnit.PlayType.OneShot)
                audioSource.PlayOneShot(soundUnit.audioClip);
            else audioSource.Play();

            return audioSource;
        }

        
    }
}


