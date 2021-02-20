using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SoundEmitter : MonoBehaviour
    {
        public AudioClip clip = null;
        public AudioSource source = null;

        public enum TypeOfSound { TwoD, Spatial }

        public TypeOfSound typeOfSound = TypeOfSound.TwoD;

        private void Start()
        {
            if (!source) source = GetComponent<AudioSource>();
        }

        public void ExecuteSound()
        {
            if (!source)
            {
                    return;
            }

            Debug.Log(clip.name);
            source.PlayOneShot(clip);
            //switch (typeOfSound)
            //{
            //    case TypeOfSound.TwoD:
            //        SoundManager.Instance.PlaySound2D(clip);
            //        break;
            //    case TypeOfSound.Spatial:
            //        SoundManager.Instance.PlaySoundPerDistanceFromPlayer(clip, transform);
            //        break;
            //}
        }
    }
}
