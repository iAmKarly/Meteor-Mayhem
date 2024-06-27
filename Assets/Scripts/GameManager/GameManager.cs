using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Tooltip("The dodge script.")]
    [SerializeField] private Dodge dodge;
    [Tooltip("The unit manager script.")]
    [SerializeField] private GameObject unitManagers;
    [Tooltip("The input manager script.")]
    [SerializeField] private GameObject inputManagers;
    [Tooltip("The canvas for game over ui.")]
    [SerializeField] private GameObject gameOverObj;

    private int state = 1;
    // 1 = on progress
    // 2 = game over
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOverObj.activeInHierarchy){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else{
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        switch(state){
            case 1:
                break;
            case 2:
                break;
        }
        
    }

    public void gameOver(){
        unitManagers.SetActive(false);
        inputManagers.SetActive(false);
        state = 2;
        gameOverObj.SetActive(true);
    }

    public void restartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void openMainMenu(){
        SceneManager.LoadScene("Home Screen");
    }
}
