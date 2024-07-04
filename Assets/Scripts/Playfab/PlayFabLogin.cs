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
            PlayFabSettings.TitleId = "C7E16"; // Please change this value to your own titleId from PlayFab Game Manager
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

        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest {DisplayName = userName}, OnDisplayName, OnLoginFailure);
    }

    void OnDisplayName(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log(result.DisplayName + " is your new display name");
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

    public GameObject LeaderboardPanel;
    public GameObject listingPrefab;
    public Transform listingContainer;
    #region Leaderboard

    public void GetLeaderboarder()
    {
        var requestLeaderboard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "Player High Score", MaxResultsCount = 10};
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeaderboard, OnErrorLeaderboard);
    }

    
    public void ViewPersonalBest()
    {
        var request = new GetLeaderboardAroundPlayerRequest { StatisticName = "Player High Score", MaxResultsCount = 1};
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
        Debug.LogError("Error retrieving personal best.");
        Debug.LogError(error.GenerateErrorReport());
    }
    

    void OnGetLeaderboard(GetLeaderboardResult result)
    {
        LeaderboardPanel.SetActive(true);

        foreach(PlayerLeaderboardEntry player in result.Leaderboard)
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
        for(int i = listingContainer.childCount - 1; i >=0; i--)
        {
            Destroy(listingContainer.GetChild(i).gameObject);
        }
    }

    void OnErrorLeaderboard(PlayFabError error)
    {
        Debug.Log("Could Not Find Player High Score");
        Debug.LogError(error.GenerateErrorReport());
    }

    #endregion Leaderboard
}