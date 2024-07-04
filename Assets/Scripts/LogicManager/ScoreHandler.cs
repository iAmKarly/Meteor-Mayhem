using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour
{
    [Tooltip("The test that displays the score.")]
    [SerializeField] private Text scoreText;
    [Tooltip("The score that the player currently has.")]
    [SerializeField] private int playerScore;

    /// <summary>
    /// Add score and display it on the UI.
    /// </summary>
    /// <param name="score">The ammount added to the score.</param>
    public void addScore(int score){
        playerScore = playerScore + score;
        scoreText.text  = playerScore.ToString();
    }
}
