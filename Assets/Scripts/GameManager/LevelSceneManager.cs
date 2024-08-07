using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSceneManager : MonoBehaviour
{
    /// <summary>
    /// Load the scene named "Playthrough1"
    /// </summary>
    public void StartGameBeginner()
    {
        SceneManager.LoadScene("Playthrough1");
    }

    /// <summary>
    /// Load the scene named "Playthrough2"
    /// </summary>
    public void StartGameIntermediate()
    {
        SceneManager.LoadScene("Playthrough2");
    }

    /// <summary>
    /// Load the scene named "Playthrough3"
    /// </summary>
    public void StartGameAdvanced()
    {
        SceneManager.LoadScene("Playthrough3");
    }

    /// <summary>
    /// Load the scene named "Tutorial"
    /// </summary>
    public void OpenTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}