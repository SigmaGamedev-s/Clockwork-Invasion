using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // глобальный доступ

    [Header("Audio Mixer")]
    public AudioMixer mainMixer;

    private void Awake()
    {
        // Проверяем, есть ли уже экземпляр
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // <-- делает объект глобальным
        }
        else
        {
            Destroy(gameObject); // не дублируем
        }
    }

    // Установка громкости (значение от 0 до 1)
    public void SetMusicVolume(float value)
    {
        // громкость в децибелы (-80dB до 0dB)
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat("VolumeMusic", dB);
        PlayerPrefs.SetFloat("VolumeMusic", value);
    }

    public void SetSFXVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        mainMixer.SetFloat("VolumeSfx", dB);
        PlayerPrefs.SetFloat("VolumeSfx", value);
    }

    // При запуске загружаем сохранённые настройки
    private void Start()
    {
        float music = PlayerPrefs.GetFloat("VolumeMusic", 1f);
        float sfx = PlayerPrefs.GetFloat("VolumeSfx", 1f);

        SetMusicVolume(music);
        SetSFXVolume(sfx);
    }
}
