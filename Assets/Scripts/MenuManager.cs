using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject settingsPanel;
    public GameObject howToPlayPanel;

    private void Awake()
    {
        settingsPanel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("levelSave"));
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void OpenHowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
    }
}
