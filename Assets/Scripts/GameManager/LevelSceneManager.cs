using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject inputManagers;
    [Tooltip("The canvas for game over ui.")]
    // This method will be called when the button is clicked
    public void StartGameBeginner()
    {
        // Load the scene named "Playthrough1"
        inputManagers.SetActive(false);
        SceneManager.LoadScene("Playthrough1");
    }
}