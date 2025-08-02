using System;
using UnityEngine;

namespace DevNote
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleStopListener : MonoBehaviour
    {
        public event Action OnStopped;


        private void OnParticleSystemStopped() => OnStopped?.Invoke();
    }
}



