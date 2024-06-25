using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
    private string userPassword;
    private string userName;
    //public GameObject loginPanel;

    public void Start()
    {
        // Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already.
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "45153"; // Please change this value to your own titleId from PlayFab Game Manager
        }

        if (PlayerPrefs.HasKey("USERNAME") && PlayerPrefs.HasKey("PASSWORD"))
        {
            userName = PlayerPrefs.GetString("USERNAME");
            userPassword = PlayerPrefs.GetString("PASSWORD");
            var request = new LoginWithPlayFabRequest { Username = userName, Password = userPassword };
            PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
        }
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString("USERNAME", userName);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        //loginPanel.SetActive(false);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call. :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    public void GetUserPassword(string passwordIn)
    {
        userPassword = passwordIn;
    }

    public void GetUsername(string usernameIn)
    {
        userName = usernameIn;
    }

    public void OnClickLogIn()
    {
        var request = new LoginWithPlayFabRequest { Username = userName, Password = userPassword };
        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        // Save username and password
        PlayerPrefs.SetString("USERNAME", userName);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        //loginPanel.SetActive(false);
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    public void OnClickRegisterUser()
    {
        var registerRequest = new RegisterPlayFabUserRequest
        {
            Username = userName,
            Password = userPassword,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterFailure);
    }
}
