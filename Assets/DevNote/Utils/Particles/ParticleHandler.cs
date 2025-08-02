using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevNote
{
    public class ParticleHandler : MonoBehaviour
    {
        public event Action OnParticleStopped;

        [SerializeField] private List<ParticleSystem> _particles;
        [SerializeField] private ParticleStopListener _stopListener;


        private void Start() => _stopListener.OnStopped += OnParticleStopped;


        public void Play() => _particles.ForEach(particle => particle.Play());

        public void Pause() => _particles.ForEach(particle => particle.Pause());

        public void Stop() => _particles.ForEach(particle => particle.Stop());

    }
}



