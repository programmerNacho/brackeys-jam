using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class SoundEmitter : MonoBehaviour
    {
        public AudioClip clip = null;

        public enum TypeOfSound { TwoD, Spatial }

        public TypeOfSound typeOfSound = TypeOfSound.TwoD;

        public void ExecuteSound()
        {
            switch (typeOfSound)
            {
                case TypeOfSound.TwoD:
                    SoundManager.Instance.PlaySound2D(clip);
                    break;
                case TypeOfSound.Spatial:
                    SoundManager.Instance.PlaySoundPerDistanceFromPlayer(clip, transform);
                    break;
            }
        }
    }
}
