using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    public Button button1;
    public Button button2;

    void Start()
    {
        button1.onClick.AddListener(OnButtonClick);
        button2.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
    }
}
