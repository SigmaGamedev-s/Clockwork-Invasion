using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinManager : Sounds
{
    public Animator WinAnimation;

    public void Win()
    {
        PlaySound(0, 0.5f);
        PlayerPrefs.SetInt("levelSave", SceneManager.GetActiveScene().buildIndex + 1);
        WinAnimation.Play("win");
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;

        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextLevel >= SceneManager.sceneCountInBuildSettings)
            nextLevel = 0;

        SceneManager.LoadScene(nextLevel);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
