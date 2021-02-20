using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Game
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            DontDestroyOnLoad(this);
        }

        [SerializeField]
        private AudioMixer gameMixer = null;
        [SerializeField]
        private List<AudioSource> fxAudioSources = new List<AudioSource>();
        [SerializeField]
        private float maxDistance = 200;

        private float volumeFX = 0f;
        private float volumeMusic = 0f;

        private PlayerInput playerCore = null;

        public float VolumeFX
        {
            get
            {
                return volumeFX;
            }

            set
            {
                volumeFX = Mathf.Log10(value) * 20.0f;
                //volumeFX = Mathf.Clamp(Mathf.Log10(value) * 20.0f, -80f, 0f);
                gameMixer.SetFloat("VolumeFX", volumeFX);
            }
        }

        public float VolumeMusic
        {
            get
            {
                return volumeMusic;
            }

            set
            {
                volumeMusic = Mathf.Log10(value) * 20.0f;
                //volumeMusic = Mathf.Clamp(value, -80f, 0f);
                gameMixer.SetFloat("VolumeMusic", volumeMusic);
            }
        }

        public void PlaySound2D(AudioClip clip)
        {
            AudioSource audioSource = GetEmptyAudioSource();

            if(audioSource)
            {
                audioSource.volume = 1f;
                StartCoroutine(ExecuteFX(audioSource, clip));
            }
        }

        public void PlaySoundPerDistanceFromPlayer(AudioClip clip, Transform emitter)
        {
            if(playerCore == null)
            {
                playerCore = FindObjectOfType<PlayerInput>();
                if(playerCore == null)
                {
                    return;
                }
            }

            float distanceFromPlayer = Vector2.Distance(emitter.transform.position, playerCore.transform.position);

            if(distanceFromPlayer < maxDistance)
            {
                AudioSource audioSource = GetEmptyAudioSource();

                if (audioSource)
                {
                    audioSource.volume = 1 - distanceFromPlayer / maxDistance;
                    StartCoroutine(ExecuteFX(audioSource, clip));
                }
            }
        }

        private AudioSource GetEmptyAudioSource()
        {
            foreach (AudioSource s in fxAudioSources)
            {
                if(s.clip == null)
                {
                    return s;
                }
            }

            return null;
        }

        private IEnumerator ExecuteFX(AudioSource audioSource, AudioClip audioClip)
        {
            audioSource.clip = audioClip;
            audioSource.Play();
            yield return new WaitForSeconds(audioClip.length);
            audioSource.clip = null;
        }
    }
}