using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;

public class PlayfabManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text messageText;

    public void RegisterButton()
    {
        var registerRequest = new RegisterPlayFabUserRequest
        {
            Username = usernameInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterError);
    }

    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registration successful!";
    }

    public void LoginButton()
    {
        var loginRequest = new LoginWithPlayFabRequest
        {
            Username = usernameInput.text,
            Password = passwordInput.text
        };

        PlayFabClientAPI.LoginWithPlayFab(loginRequest, OnLoginSuccess, OnLoginError);
    }

    void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "Login successful!";
        messageText.color = new Color(0.384f, 0.925f, 0.173f, 1.0f);
        print("Success");
    }

    void OnLoginError(PlayFabError error)
    {
        messageText.text = "Login failed!";
        messageText.color = new Color(1f, 0f, 0f, 1.0f);
        print("Error: " + error.ErrorMessage);
    }

    void OnRegisterError(PlayFabError error)
    {
        messageText.text = "Sign Up failed!";
        messageText.color = new Color(1f, 0f, 0f, 1.0f);
        print("Error: " + error.ErrorMessage);
    }
}

