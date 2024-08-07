using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;

public class AccessManager : MonoBehaviour
{
    [Tooltip("The text box for username.")]
    [SerializeField] private TMP_InputField usernameInput;
    [Tooltip("The text box for password.")]
    [SerializeField] private TMP_InputField passwordInput;
    [Tooltip("The The text box for display name.")]
    [SerializeField] private TMP_InputField displayNameInput;
    [Tooltip("The The text for message")]
    [SerializeField] private TMP_Text messageText;

    [Tooltip("The rows for the leaderboard table")]
    [SerializeField] private GameObject rowPrefab;
    [Tooltip("The table for leaderboad.")]
    [SerializeField] private Transform rowsParent;

    void Start(){
        Time.timeScale = 1;
        if(rowPrefab != null && rowsParent != null){
            GetLeaderboard();
        }
        if(!IsPlayerLoggedIn()){
            login();
        }
    }

    /// <summary>
    /// Logs in player with the existing environment variable
    /// </summary>
    public void login(){
        string username = PlayerPrefs.GetString("USERNAME");
        string password = PlayerPrefs.GetString("password");
            var loginRequest = new LoginWithPlayFabRequest
            {
                Username = username,
                Password = password
            };

        if (username.Length > 0 && password.Length > 0){
            PlayFabClientAPI.LoginWithPlayFab(loginRequest, OnLoginSuccess, OnLoginError);
        }
        else{
            print("Error no username and password");
        }
        
    }

    /// <summary>
    /// Logs in player with the text boxes from user input
    /// </summary>
    public void LoginButton()
    {
        var loginRequest = new LoginWithPlayFabRequest
        {
            Username = usernameInput.text,
            Password = passwordInput.text
        };

        PlayFabClientAPI.LoginWithPlayFab(loginRequest, OnLoginSuccess, OnLoginError);
    }

    /// <summary>
    /// Display success message for logged in and saves the data as an environment variable
    /// </summary>
    void OnLoginSuccess(LoginResult result)
    {
        messageText.text = "Login successful!";
        messageText.color = new Color(0.384f, 0.925f, 0.173f, 1.0f);
        PlayerPrefs.SetString("USERNAME", usernameInput.text);
        PlayerPrefs.SetString("PASSWORD", passwordInput.text);
        PlayerPrefs.Save();
        print("Success");
        StartCoroutine(openHome(5f));
    }

    /// <summary>
    /// Display failed message for logging in
    /// </summary>
    void OnLoginError(PlayFabError error)
    {
        messageText.text = "Login failed!";
        messageText.color = new Color(1f, 0f, 0f, 1.0f);
        print("Error: " + error.ErrorMessage);
    }

    /// <summary>
    /// Retursn if a player is logged in
    /// </summary>
    public static bool IsPlayerLoggedIn()
    {
        // return PlayerPrefs.HasKey("USERNAME") && PlayerPrefs.HasKey("PASSWORD");
        return PlayFabClientAPI.IsClientLoggedIn();
    }

    /// <summary>
    /// Registers the user based on the user input
    /// </summary>
    public void RegisterButton()
    {
        var registerRequest = new RegisterPlayFabUserRequest
        {
            Username = usernameInput.text,
            Password = passwordInput.text,
            DisplayName = displayNameInput.text,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterError);
    }

    /// <summary>
    /// Display message and stores the user input when success in registration
    /// </summary>
    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registration successful!";
        messageText.color = new Color(0.384f, 0.925f, 0.173f, 1.0f);
        PlayerPrefs.SetString("USERNAME", usernameInput.text);
        PlayerPrefs.SetString("PASSWORD", passwordInput.text);
        PlayerPrefs.Save();
        print("Registration successful");
        StartCoroutine(openHome(5f));
    }

    /// <summary>
    /// Display message when registration fails
    /// </summary>
    void OnRegisterError(PlayFabError error)
    {
        messageText.text = "Sign Up failed!";
        messageText.color = new Color(1f, 0f, 0f, 1.0f);
        print("Error: " + error.ErrorMessage);
    }

    /// <summary>
    /// Logs out the player and delete the local data
    /// </summary>
    public static void Logout()
    {
        PlayerPrefs.DeleteKey("USERNAME");
        PlayerPrefs.DeleteKey("PASSWORD");
        PlayerPrefs.Save();
        print("Logged out successfully");
    }

    /// <summary>
    /// Redirect to the home screen
    /// </summary>
    public IEnumerator<object> openHome(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        MainMenuSceneManager.OpenHome();
    }

    /// <summary>
    /// Sends the new score to the leaderboard and updates it
    /// </summary>
    public void SendLeaderBoard(int score){
        if(IsPlayerLoggedIn()){
            var request = new UpdatePlayerStatisticsRequest{
                Statistics = new List<StatisticUpdate>{
                    new StatisticUpdate{
                        StatisticName = "Player High Score",
                        Value = score
                    }
                }
            };
            PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnErrorLeaderboard);
        }
    }

    /// <summary>
    /// Retrieves the leaderboard data
    /// </summary>
    [ContextMenu("GetLeaderboards")]
    public void GetLeaderboard()
    {
        var requestLeaderboard = new GetLeaderboardRequest
        {
            StartPosition = 0,
            StatisticName = "Player High Score",
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeaderboard, OnErrorLeaderboard);
    }

    /// <summary>
    /// Displays the leaderboard data
    /// </summary>
    void OnGetLeaderboard(GetLeaderboardResult result)
    {
        foreach (Transform player in rowsParent){
            Destroy(player.gameObject);
        }
        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPrefab, rowsParent);
            TMP_Text[] texts = newGo.GetComponentsInChildren<TMP_Text>();
            texts[0].text = (player.Position + 1).ToString();
            texts[1].text = player.DisplayName;
            texts[2].text = player.StatValue.ToString();
        }
    }

    /// <summary>
    /// Displays error message when failing to retrieve leaderboard data
    /// </summary>
    void OnErrorLeaderboard(PlayFabError error)
    {
        print(error.GenerateErrorReport());
    }

    /// <summary>
    /// Displays success message when succeeding to retrieve leaderboard data
    /// </summary>
    void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result){
        print("Leaderboard updated!");
    }
}

