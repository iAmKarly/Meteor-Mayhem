using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject heart0, heart1, heart2, heart3, gameOver;
    public static int health;

    void Start()
    {
        health = 4;
        heart0.SetActive(true);
        heart1.SetActive(true);
        heart2.SetActive(true);
        heart3.SetActive(true);
        gameOver.SetActive(false);
    }

    public void LoseHeart()
    {
        health--; // Decrease health

        // Update heart images visibility based on current health
        switch (health)
        {
            case 3:
                heart3.SetActive(false);
                break;
            case 2:
                heart2.SetActive(false);
                break;
            case 1:
                heart1.SetActive(false);
                break;
            case 0:
                heart0.SetActive(false);
                gameOver.SetActive(true);
                Time.timeScale = 0;
                break;
            default:
                break;
        }
    }
}
