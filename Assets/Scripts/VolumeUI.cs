using UnityEngine;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            musicSlider.value = PlayerPrefs.GetFloat("VolumeMusic", 1f);
            sfxSlider.value = PlayerPrefs.GetFloat("VolumeSfx", 1f);
        }

        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
    }

    private void OnMusicChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(value);
    }

    private void OnSFXChanged(float value)
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetSFXVolume(value);
    }
}
