using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    private void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        // Проверка на ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // ⏸ Включаем паузу
    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f; // ставим время на паузу
        isPaused = true;
    }

    // ▶ Продолжаем игру
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f; // возвращаем время
        isPaused = false;
    }

    // 🔁 Начать заново (перезапуск текущей сцены)
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isPaused = false;
    }

    // 🏠 Вернуться в главное меню
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
}
