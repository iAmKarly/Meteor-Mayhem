using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ErrorMessage : MonoBehaviour
{
    public TextMeshProUGUI errorMessage;
    public Button closeButton;

    private PlayFabLogin playFabLogin;

    private void Awake()
    {
        playFabLogin = FindObjectOfType<PlayFabLogin>();
        closeButton.onClick.AddListener(CloseErrorPanel);
    }

    private void CloseErrorPanel()
    {
        if (playFabLogin != null)
        {
            playFabLogin.CloseErrorPanel();
        }
    }
}
