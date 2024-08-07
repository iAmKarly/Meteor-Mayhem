using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MainMenuSceneManager : MonoBehaviour
{
    [Tooltip("The menu screen obj.")]
    [SerializeField] private GameObject mainMenuObj;
    [Tooltip("The confirmation screen obj.")]
    [SerializeField] private GameObject confirmationObj;
    [Tooltip("The not logged in screen obj.")]
    [SerializeField] private GameObject NotloggedInObj;
    [Tooltip("The logged in screen obj.")]
    [SerializeField] private GameObject loggedInObj;

    /// <summary>
    /// Load the scene named "Home Screen"
    /// </summary>
    public static void OpenHome()
    {
        SceneManager.LoadScene("Home Screen");
    }

    /// <summary>
    /// Load the scene named "ChooseLevel"
    /// </summary>
    public void ChangeToChooseLevel()
    {
        SceneManager.LoadScene("ChooseLevel");
    }

    /// <summary>
    /// Load the scene named "Settings"
    /// </summary>
    public void OpenSettings()
    {
        // Load the scene named "Settings"
        SceneManager.LoadScene("Settings");
    }

    /// <summary>
    /// Load the scene named "Leaderboard"
    /// </summary>
    public void OpenLeaderboard()
    {
        if(AccessManager.IsPlayerLoggedIn()){
            // Load the scene named "Leaderboard"
            SceneManager.LoadScene("Leaderboard");
        }
        else{
            StartCoroutine(displayNotLoggedIn(5));
        }
    }

    /// <summary>
    /// Load the scene named "CreateAccount"
    /// </summary>
    public void OpenRegister()
    {
        if(!AccessManager.IsPlayerLoggedIn()){
            SceneManager.LoadScene("CreateAccountPage");
        }
        else{
            StartCoroutine(displayLoggedIn(5));
        }
    }

    /// <summary>
    /// Load the scene named "LoginPage"
    /// </summary>
    public void OpenLogin()
    {
        SceneManager.LoadScene("LoginPage");
    }

    /// <summary>
    /// Opens the quit application confirmation screen
    /// </summary>
    public void QuitApplicationConfirmation()
    {
        confirmationObj.SetActive(true);
        mainMenuObj.SetActive(false);
    }

    /// <summary>
    /// Quits the application
    /// </summary>
    public void QuitConfirmation()
    {
        AccessManager.Logout();
        Application.Quit();
    }

    /// <summary>
    /// Close the quit application confirmation screen
    /// </summary>
    public void QuitCancel()
    {
        confirmationObj.SetActive(false);
        mainMenuObj.SetActive(true);
    }

    /// <summary>
    /// Display the not logged in alert
    /// </summary>
    public IEnumerator<object> displayNotLoggedIn(float seconds)
    {
        NotloggedInObj.SetActive(true);
        yield return new WaitForSeconds(seconds);
        NotloggedInObj.SetActive(false);
    }

    /// <summary>
    /// Display the logged in alert
    /// </summary>
    public IEnumerator<object> displayLoggedIn(float seconds)
    {
        loggedInObj.SetActive(true);
        yield return new WaitForSeconds(seconds);
        loggedInObj.SetActive(false);
    }
}