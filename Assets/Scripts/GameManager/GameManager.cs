using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Tooltip("The unit manager script.")]
    [SerializeField] private GameObject unitManagers;
    [Tooltip("The mouse manager script.")]
    [SerializeField] private GameObject mouseManager;
    [Tooltip("The mouse manager script.")]
    [SerializeField] private GameObject handManager;
    [Tooltip("The canvas for game over ui.")]
    [SerializeField] private GameObject gameOverObj;
    [Tooltip("The canvas for pause ui.")]
    [SerializeField] private GameObject pauseObj;
    [Tooltip("The confirmation screen obj.")]
    [SerializeField] private GameObject confirmationObj;
    [Tooltip("The score handler.")]
    [SerializeField] private ScoreHandler scoreHandler;
    [Tooltip("The access manager.")]
    [SerializeField] private AccessManager accessManager;

    private bool pausing = false;
    private bool died = false;
    private HandTracker handTracker;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        died = false;
        handTracker = handManager.GetComponent<HandTracker>();
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOverObj.activeInHierarchy || pauseObj.activeInHierarchy || confirmationObj.activeInHierarchy){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            handTracker.disableHandObj("Left");
            handTracker.disableHandObj("Right");
        }
        else{
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseGame();
        }   
    }

    /// <summary>
    /// Stops the game playthrough functions, Displays the game over screen, Update the leaderboard
    /// </summary>
    public void gameOver(){
        unitManagers.SetActive(false);
        mouseManager.SetActive(false);
        Time.timeScale = 0;
        gameOverObj.SetActive(true);
        died = true;
        accessManager.SendLeaderBoard(scoreHandler.getScore());
    }

    /// <summary>
    /// Stops the game playthrough functions, Displays the pause screen
    /// </summary>
    public void pauseGame()
    {
        if(!died)
        {
            unitManagers.SetActive(false);
            mouseManager.SetActive(false);
            Time.timeScale = 0;
            pauseObj.SetActive(true);
            pausing = true;
        }
    }

    /// <summary>
    /// Resume the game playthrough functions, Closes pause screen
    /// </summary>
    public void resumeGame()
    {
        unitManagers.SetActive(true);
        mouseManager.SetActive(true);
        Time.timeScale = 1;
        pauseObj.SetActive(false);
        pausing = false;
        
        handTracker.enableHandObj("Left");
        handTracker.enableHandObj("Right");
    }

    /// <summary>
    /// Restart the game playthrough scene
    /// </summary>
    public void restartGame(){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Open the quit to main menu confirmation screen
    /// </summary>
    public void openMainMenuConfirmation(){
        pauseObj.SetActive(false);
        gameOverObj.SetActive(false);
        confirmationObj.SetActive(true);
    }

    /// <summary>
    /// Redirect to the main menu
    /// </summary>
    public void QuitConfirmation()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Home Screen");
    }

    /// <summary>
    /// Returns to the original screen before selecting return to main menu
    /// </summary>
    public void QuitCancel()
    {
        if(pausing){
            pauseObj.SetActive(true);
        }
        else{
            gameOverObj.SetActive(true);
        }
        confirmationObj.SetActive(false);
    }
}
