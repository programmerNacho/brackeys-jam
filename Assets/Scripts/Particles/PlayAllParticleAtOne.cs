using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class PlayAllParticleAtOne : MonoBehaviour
    {
        private ParticleSystem[] particles = null;

        private void Start()
        {
            particles = GetComponentsInChildren<ParticleSystem>();
        }

        public void Play()
        {
            foreach (ParticleSystem p in particles)
            {
                p.Play();
            }
        }
    }
}
