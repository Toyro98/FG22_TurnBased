using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        GameManager.IsGamePaused = false;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
        GameManager.IsGamePaused = false;
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(2);
    }
}
