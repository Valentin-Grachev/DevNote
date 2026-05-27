using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace DevNote
{

    public class Sound : MonoBehaviour, IInitializable
    {
        public enum Channel { Music, SFX }
        public static bool Initialized { get; private set; }


        public class Settings
        {
            public static void SetMasterVolume(float volume)
            {
                volume = Mathf.Clamp(volume, 0.0001f, 1f);
                float db = Mathf.Log10(volume) * 20f;

                if (!_instance._audioMixer.GetFloat("masterVolume", out _))
                    Debug.LogWarning("Your audio mixer doesn't contain parameter \"masterVolume\"!");

                _instance._audioMixer.SetFloat("masterVolume", Mathf.Clamp(db, -80f, 20f));
            }



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
        [SerializeField] private AudioSource _musicAudioSource;
        [SerializeField] private AudioPool _sfxAudioPool;

        private AudioMixerGroup _sfxGroup;
        private float _originMusicVolume;
        

        bool IInitializable.Initialized => Initialized;

        void IInitializable.Initialize()
        {
            _instance = this;

            _sfxGroup = _audioMixer.FindMatchingGroups("SFX")[0];
            _musicAudioSource.outputAudioMixerGroup = _audioMixer.FindMatchingGroups("Music")[0];

            Settings.Apply();

            Initialized = true;
        }

        
        public static void SetMusicVolume(float volume, float softDuration = 0f)
        {
            var audioSource = _instance._musicAudioSource;

            float targetVolume = volume * _instance._originMusicVolume;

            DOTween.To(() => audioSource.volume, x => audioSource.volume = x,
                targetVolume, softDuration).SetEase(Ease.OutQuad);
        }


        public static async UniTask<AudioSource> Play(SoundUnit soundUnit)
        {
            AudioSource audioSource = soundUnit.channel == Channel.Music ? 
                _instance._musicAudioSource : _instance._sfxAudioPool.GetAudioSource();

            var clip = await soundUnit.GetAudioClip();
            audioSource.clip = clip;

            if (soundUnit.channel == Channel.SFX)
                audioSource.outputAudioMixerGroup = _instance._sfxGroup;

            audioSource.volume = soundUnit.Volume;

            if (soundUnit.channel == Channel.Music)
                _instance._originMusicVolume = soundUnit.Volume;

            audioSource.loop = soundUnit.playType == SoundUnit.PlayType.Loop;
            audioSource.pitch = soundUnit.Pitch;

            if (soundUnit.playType == SoundUnit.PlayType.OneShot)
                audioSource.PlayOneShot(clip);

            else audioSource.Play();

            return audioSource;
        }


    }
}


