using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartHandler : MonoBehaviour
{
    [Tooltip("The canvas for hearts.")]
    [SerializeField] private Canvas hearts;
    [Tooltip("The current number of hearts.")]
    [SerializeField] private int currentHearts = 3;
    [Tooltip("The game manager.")]
    [SerializeField] private GameManager gameManager;

    private bool isDead = false;

    /// <summary>
    /// Update the heart canvas based one the current heart the player has
    /// </summary>
    void updateHearts(){
        switch(currentHearts){
            case 3:{
                GameObject heart3 = hearts.transform.GetChild(2).GetChild(1).gameObject;
                CanvasGroup heart3CanvasGroup = heart3.GetComponent<CanvasGroup>();
                heart3CanvasGroup.alpha = 1;
                GameObject heart2 = hearts.transform.GetChild(1).GetChild(1).gameObject;
                CanvasGroup heart2CanvasGroup = heart2.GetComponent<CanvasGroup>();
                heart2CanvasGroup.alpha = 1;
                GameObject heart1 = hearts.transform.GetChild(0).GetChild(1).gameObject;
                CanvasGroup heart1CanvasGroup = heart1.GetComponent<CanvasGroup>();
                heart1CanvasGroup.alpha = 1;
                break;
            }       
            case 2:{
                GameObject heart3 = hearts.transform.GetChild(2).GetChild(1).gameObject;
                CanvasGroup heart3CanvasGroup = heart3.GetComponent<CanvasGroup>();
                heart3CanvasGroup.alpha = 0;
                GameObject heart2 = hearts.transform.GetChild(1).GetChild(1).gameObject;
                CanvasGroup heart2CanvasGroup = heart2.GetComponent<CanvasGroup>();
                heart2CanvasGroup.alpha = 1;
                GameObject heart1 = hearts.transform.GetChild(0).GetChild(1).gameObject;
                CanvasGroup heart1CanvasGroup = heart1.GetComponent<CanvasGroup>();
                heart1CanvasGroup.alpha = 1;
                break;
            }
            case 1:{
                GameObject heart3 = hearts.transform.GetChild(2).GetChild(1).gameObject;
                CanvasGroup heart3CanvasGroup = heart3.GetComponent<CanvasGroup>();
                heart3CanvasGroup.alpha = 0;
                GameObject heart2 = hearts.transform.GetChild(1).GetChild(1).gameObject;
                CanvasGroup heart2CanvasGroup = heart2.GetComponent<CanvasGroup>();
                heart2CanvasGroup.alpha = 0;
                GameObject heart1 = hearts.transform.GetChild(0).GetChild(1).gameObject;
                CanvasGroup heart1CanvasGroup = heart1.GetComponent<CanvasGroup>();
                heart1CanvasGroup.alpha = 1;
                break;
            }          
            default:{
                GameObject heart3 = hearts.transform.GetChild(2).GetChild(1).gameObject;
                CanvasGroup heart3CanvasGroup = heart3.GetComponent<CanvasGroup>();
                heart3CanvasGroup.alpha = 0;
                GameObject heart2 = hearts.transform.GetChild(1).GetChild(1).gameObject;
                CanvasGroup heart2CanvasGroup = heart2.GetComponent<CanvasGroup>();
                heart2CanvasGroup.alpha = 0;
                GameObject heart1 = hearts.transform.GetChild(0).GetChild(1).gameObject;
                CanvasGroup heart1CanvasGroup = heart1.GetComponent<CanvasGroup>();
                heart1CanvasGroup.alpha = 0;
                if(!isDead){
                    isDead = true;
                    gameManager.gameOver();
                }
                break;
            }     
        }
    }

    /// <summary>
    /// Decrease the player health
    /// </summary>
    [ContextMenu("Decrease Hearts")]
    public void decreaseHearts(){
        if (currentHearts > 0)
            currentHearts -= 1;
        updateHearts();
    }

    /// <summary>
    /// Increase the player health
    /// </summary>
    [ContextMenu("Increase Hearts")]
    public void increaseHearts(){
        if (currentHearts < 3)
            currentHearts += 1;
        updateHearts();
    }
}
