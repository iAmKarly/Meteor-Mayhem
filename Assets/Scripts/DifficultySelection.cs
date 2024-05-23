using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class DifficultySelection : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("SelectDifficulty");
    }
}
