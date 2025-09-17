using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace DevNote
{

    public class Sound : MonoBehaviour, IInitializable
    {
        public enum Channel { Music, SFX }
        public static bool Initialized => _instance != null;


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

        private AudioMixerGroup _sfxGroup;
        private AudioSource _musicAudioSource;

        bool IInitializable.Initialized => Initialized;

        void IInitializable.Initialize()
        {
            _instance = this;

            _sfxGroup = _audioMixer.FindMatchingGroups("SFX")[0];

            _musicAudioSource = _sfxAudioPool.GetAudioSource();
            _musicAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Music")[0];

            Settings.Apply();
        }



        public static AudioSource Play(SoundUnit soundUnit)
        {
            AudioSource audioSource = soundUnit.channel == Channel.Music ? 
                _instance._musicAudioSource : _instance._sfxAudioPool.GetAudioSource();

            audioSource.clip = soundUnit.audioClip;

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


