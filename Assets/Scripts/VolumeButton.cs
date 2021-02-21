using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeButton : MonoBehaviour
{
    private static string musicOn = "Music On";
    private static string musicOff = "Music Off";
    private static string fxOn = "FX On";
    private static string fxOff = "FX Off";

    public enum TypeOfVolume { FX, Music }

    public TypeOfVolume typeOfVolume = TypeOfVolume.FX;
    [SerializeField]
    private TextMeshProUGUI text = null;

    private Button button = null;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ButtonClicked);
    }

    private void OnEnable()
    {
        switch (typeOfVolume)
        {
            case TypeOfVolume.FX:
                text.text = Game.SoundManager.Instance.VolumeFX == -80f ? fxOff : fxOn;
                break;
            case TypeOfVolume.Music:
                text.text = Game.SoundManager.Instance.VolumeMusic == -80f ? musicOff : musicOn;
                break;
        }
    }

    private void ButtonClicked()
    {
        switch (typeOfVolume)
        {
            case TypeOfVolume.FX:
                Game.SoundManager.Instance.FXChange();
                text.text = Game.SoundManager.Instance.VolumeFX == -80f ? fxOff : fxOn;
                break;
            case TypeOfVolume.Music:
                Game.SoundManager.Instance.MusicChange();
                text.text = Game.SoundManager.Instance.VolumeMusic == -80f ? musicOff : musicOn;
                break;
        }
    }
}
