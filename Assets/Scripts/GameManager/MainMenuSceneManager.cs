using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneManager : MonoBehaviour
{
    [Tooltip("The canvas for game over ui.")]
    // This method will be called when the button is clicked
    public void ChangeToChooseLevel()
    {
        // Load the scene named "ChooseLevel"
        SceneManager.LoadScene("ChooseLevel");
    }
    public void StartGameBeginner()
    {
        // Load the scene named "Playthrough1"
        SceneManager.LoadScene("Playthrough1");
    }
    public void OpenSettings()
    {
        // Load the scene named "Settings"
        SceneManager.LoadScene("Settings");
    }
    public void OpenLeaderboard()
    {
        // Load the scene named "Settings"
        SceneManager.LoadScene("Leaderboard");
    }
    public void OpenLogin()
    {
        // Load the scene named "Settings"
        SceneManager.LoadScene("Login");
    }
    public void QuitApplication()
    {
        Application.Quit();
    }
}