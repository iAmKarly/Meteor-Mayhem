using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayFabLogin : MonoBehaviour
{
    private string userPassword;
    private string userName;

    public GameObject errorPanelPrefab;
    private GameObject errorPanelInstance;

    private void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "C7E16"; // Please change this value to your own titleId from PlayFab Game Manager
        }

        LoadCredentials();
    }

    private void LoadCredentials()
    {
        if (PlayerPrefs.HasKey("USERNAME") && PlayerPrefs.HasKey("PASSWORD"))
        {
            userName = PlayerPrefs.GetString("USERNAME");
            userPassword = PlayerPrefs.GetString("PASSWORD");

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(userPassword))
            {
                var request = new LoginWithPlayFabRequest { Username = userName, Password = userPassword };
                PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
            }
        }
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login successful!");
        PlayerPrefs.SetString("USERNAME", userName);
        PlayerPrefs.SetString("PASSWORD", userPassword);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        ShowError(error.GenerateErrorReport());
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
        Debug.Log("Registration successful!");
        PlayerPrefs.SetString("USERNAME", userName);
        PlayerPrefs.SetString("PASSWORD", userPassword);

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = userName }, OnDisplayName, OnLoginFailure);
    }

    void OnDisplayName(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log(result.DisplayName + " is your new display name");
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        ShowError(error.GenerateErrorReport());
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

    public GameObject LeaderboardPanel;
    public GameObject listingPrefab;
    public Transform listingContainer;

    #region Leaderboard

    public void GetLeaderboarder()
    {
        if (!IsPlayerLoggedIn())
        {
            Debug.LogWarning("Player is not logged in. Please log in first.");
            ShowError("Player is not logged in. Please log in first.");
            return;
        }

        var requestLeaderboard = new GetLeaderboardRequest
        {
            StartPosition = 0,
            StatisticName = "Player High Score",
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeaderboard, OnErrorLeaderboard);
    }

    public void ViewPersonalBest()
    {
        if (!IsPlayerLoggedIn())
        {
            Debug.LogWarning("Player is not logged in. Please log in first.");
            ShowError("Player is not logged in. Please log in first.");
            return;
        }

        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "Player High Score",
            MaxResultsCount = 1
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetPersonalBest, OnErrorPersonalBest);
    }

    void OnGetPersonalBest(GetLeaderboardAroundPlayerResult result)
    {
        LeaderboardPanel.SetActive(true);

        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {
            GameObject tempListing = Instantiate(listingPrefab, listingContainer);
            LeaderboardListing LL = tempListing.GetComponent<LeaderboardListing>();
            LL.playerNameText.text = player.DisplayName;
            LL.playerScoreText.text = player.StatValue.ToString();
            Debug.Log(player.DisplayName + ": " + player.StatValue);
        }
    }

    void OnErrorPersonalBest(PlayFabError error)
    {
        ShowError(error.GenerateErrorReport());
    }

    void OnGetLeaderboard(GetLeaderboardResult result)
    {
        LeaderboardPanel.SetActive(true);

        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {
            GameObject tempListing = Instantiate(listingPrefab, listingContainer);
            LeaderboardListing LL = tempListing.GetComponent<LeaderboardListing>();
            LL.playerNameText.text = player.DisplayName;
            LL.playerScoreText.text = player.StatValue.ToString();
            Debug.Log(player.DisplayName + ": " + player.StatValue);
        }
    }

    public void CloseLeaderboardPanel()
    {
        LeaderboardPanel.SetActive(false);
        for (int i = listingContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(listingContainer.GetChild(i).gameObject);
        }
    }

    void OnErrorLeaderboard(PlayFabError error)
    {
        ShowError(error.GenerateErrorReport());
    }

    #endregion Leaderboard

    private bool IsPlayerLoggedIn()
    {
        return PlayerPrefs.HasKey("USERNAME") && PlayerPrefs.HasKey("PASSWORD");
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey("USERNAME");
        PlayerPrefs.DeleteKey("PASSWORD");
        Debug.Log("Logged out successfully");
    }

    private void ShowError(string errorMessage)
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        if (canvas != null)
        {
            errorPanelInstance = Instantiate(errorPanelPrefab, canvas.transform);

            RectTransform rectTransform = errorPanelInstance.GetComponent<RectTransform>();
            rectTransform.localPosition = Vector3.zero;

            ErrorMessage errorMessageComponent = errorPanelInstance.GetComponent<ErrorMessage>();
            if (errorMessageComponent != null)
            {
                errorMessageComponent.errorMessage.text = errorMessage;
            }
            else
            {
                Debug.LogError("ErrorMessage component not found on the error panel instance.");
            }
        }
        else
        {
            Debug.LogError("Canvas not found in the scene. Please ensure there is a Canvas in the scene.");
        }
    }

    public void CloseErrorPanel()
    {
        if (errorPanelInstance != null)
        {
            Destroy(errorPanelInstance);
        }
    }
}

