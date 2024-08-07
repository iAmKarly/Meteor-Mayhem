using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialSceneManager : MonoBehaviour
{
    [Tooltip("The panel that stores the pages.")]
    [SerializeField] private GameObject panel;
    [Tooltip("The text that stores the page number.")]
    [SerializeField] private TMP_Text pageCount;

    private GameObject[] pages;
    private int currentIndex = 0;


    void Start()
    {
        pages = new GameObject[panel.transform.childCount];
        for (int i = 0; i < panel.transform.childCount; i++)
        {
            pages[i] = panel.transform.GetChild(i).gameObject;
        }

        UpdateDisplay();
        GameObject.FindWithTag("leftButton").GetComponent<Button>().interactable = false;
    }
    
    /// <summary>
    /// Hides the current page and displays the next page
    /// </summary>
    [ContextMenu("Next")]
    public void Next()
    {;
        if (currentIndex < pages.Length - 1)
        {
            currentIndex++;
            UpdateDisplay();
            GameObject.FindWithTag("leftButton").GetComponent<Button>().interactable = true;
        }
        if (currentIndex >= pages.Length - 1)
        {
            GameObject.FindWithTag("rightButton").GetComponent<Button>().interactable = false;
        }
        
        
    }

    /// <summary>
    /// Hides the current page and displays the previous page
    /// </summary>
    [ContextMenu("Previous")]
    public void Previous()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateDisplay();
            GameObject.FindWithTag("rightButton").GetComponent<Button>().interactable = true;
        }
        if (currentIndex <= 0)
        {
            GameObject.FindWithTag("leftButton").GetComponent<Button>().interactable = false;
        }
    }

    /// <summary>
    /// Update the display based on the current page number
    /// </summary>
    private void UpdateDisplay()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if (i == currentIndex)
            {
                pages[i].gameObject.SetActive(true);
            }
            else
            {
                pages[i].gameObject.SetActive(false);
            }
        }
        pageCount.text = (currentIndex + 1) + " / " + (pages.Length);
    }

    /// <summary>
    /// Load the scene named "ChooseLevel"
    /// </summary>
    public void ChangeToChooseLevel()
    {
        SceneManager.LoadScene("ChooseLevel");
    }
}
