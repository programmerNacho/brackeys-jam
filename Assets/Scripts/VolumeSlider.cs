using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    public enum TypeOfVolume { FX, Music }

    public TypeOfVolume typeOfVolume = TypeOfVolume.FX;

    private Slider slider = null;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void OnEnable()
    {
        switch (typeOfVolume)
        {
            case TypeOfVolume.FX:
                slider.value = Game.SoundManager.Instance.VolumeFX;
                break;
            case TypeOfVolume.Music:
                slider.value = Game.SoundManager.Instance.VolumeMusic;
                break;
        }
    }

    public void SetVolume(float value)
    {
        switch (typeOfVolume)
        {
            case TypeOfVolume.FX:
                Game.SoundManager.Instance.VolumeFX = value;
                break;
            case TypeOfVolume.Music:
                Game.SoundManager.Instance.VolumeMusic = value;
                break;
        }
    }
}
