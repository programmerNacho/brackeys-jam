using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ParticleRequest : MonoBehaviour
    {
        public string identifier = "";

        public void RequestParticle()
        {
            ParticleCreator.Instance.CreateParticleAtPosition(identifier, transform.position);
        }
    }
}
