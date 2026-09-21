using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    public class AudioPool : MonoBehaviour
    {
        private List<AudioSource> _audioSources = new List<AudioSource>();


        public AudioSource GetAudioSource()
        {
            for (int i = 0; i < _audioSources.Count; i++)
                if (!_audioSources[i].isPlaying) return _audioSources[i];

            var newAudioSource = gameObject.AddComponent<AudioSource>();
            newAudioSource.playOnAwake = false;
            newAudioSource.volume = 1f;
            _audioSources.Add(newAudioSource);
            return newAudioSource;
        }




    }
}



