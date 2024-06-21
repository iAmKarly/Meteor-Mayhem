using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // This method will be called when the button is clicked
    public void ChangeToPlaythrough1()
    {
        // Load the scene named "Playthrough1"
        SceneManager.LoadScene("Playthrough1");
    }
}