using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ParticleCreator : MonoBehaviour
    {
        public static ParticleCreator Instance { get; private set; }

        private void Awake()
        {
            if(Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        [System.Serializable]
        public class ParticleEffect
        {
            public string identifier = "";
            public GameObject particles = null;
            public float timeToDestroy = 5f;
        }

        public List<ParticleEffect> particleEffects = new List<ParticleEffect>();

        public void CreateParticleAtPosition(string particleIdentifier, Vector3 worldPostion)
        {
            foreach (ParticleEffect p in particleEffects)
            {
                if(particleIdentifier == p.identifier)
                {
                    GameObject particle = Instantiate(p.particles, worldPostion, Quaternion.identity);
                    Destroy(particle, p.timeToDestroy);
                }
            }
        }
    }
}
