using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenevManager : MonoBehaviour
{
    // =========================
    // MAIN MENU
    // =========================
    public void MainMenu(int sceneIndex)
    {
        // Make sure the game is unpaused
        Time.timeScale = 1f;

        // Load Main Menu
        SceneManager.LoadScene(sceneIndex);
    }
     public void Retry(int sceneIndex)
    {
        // Make absolutely sure the game is unpaused
        Time.timeScale = 1f;

        // Reload the selected scene
        SceneManager.LoadScene(sceneIndex);
    }
    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
    public void BonusLevel(int sceneIndex)
    {
        // Make sure the game is unpaused
        Time.timeScale = 1f;

        // Load Bonus Level
        SceneManager.LoadScene(sceneIndex);
    }
    public void controls(int sceneIndex)
    {
        // Make sure the game is unpaused
        Time.timeScale = 1f;

        // Load Controls Scene
        SceneManager.LoadScene(sceneIndex);
    }

    // =========================
    // WHEN SCENE STARTS
    // =========================
    private void Start()
    {
        // Safety reset.
        // If another scene previously paused the game,
        // this guarantees that the new scene can run.
        Time.timeScale = 1f;
    }
}

