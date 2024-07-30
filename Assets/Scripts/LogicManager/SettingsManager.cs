using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider volumeSlider;
    public Button fullScreenButton;
    public Button windowedButton;
    public Button fps30Button;
    public Button fps60Button;
    public Button fps120Button;
    public Button saveButton;
    public Button applyButton;

    private void Start()
    {
        // Initialize UI elements with current settings
        InitializeVolume();
        InitializeDisplayModeButtons();
        InitializeFPS();

        // Add listeners for UI elements
        volumeSlider.onValueChanged.AddListener(SetVolume);
        fullScreenButton.onClick.AddListener(() => SetDisplayMode(0));
        windowedButton.onClick.AddListener(() => SetDisplayMode(1));
        fps30Button.onClick.AddListener(() => SetFPS(30));
        fps60Button.onClick.AddListener(() => SetFPS(60));
        fps120Button.onClick.AddListener(() => SetFPS(120));
        saveButton.onClick.AddListener(SaveSettings);
        applyButton.onClick.AddListener(ApplySettings);
    }

    private void InitializeVolume()
    {
        volumeSlider.value = AudioListener.volume;
    }

    private void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    private void InitializeDisplayModeButtons()
    {
        // Set the button states to reflect the current display mode
        switch (Screen.fullScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                Debug.Log("Current mode: Full Screen");
                fullScreenButton.interactable = false;
                windowedButton.interactable = true;
                break;
            case FullScreenMode.Windowed:
                Debug.Log("Current mode: Windowed");
                fullScreenButton.interactable = true;
                windowedButton.interactable = false;
                break;
        }
    }

    private void SetDisplayMode(int modeIndex)
    {
        // Set the display mode based on the button clicked
        FullScreenMode mode = FullScreenMode.Windowed;
        switch (modeIndex)
        {
            case 0:
                mode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                mode = FullScreenMode.Windowed;
                break;
        }
        Screen.fullScreenMode = mode;
        Debug.Log($"Set display mode to: {mode}");
        InitializeDisplayModeButtons(); // Update button states
    }

    private void InitializeFPS()
    {
        int currentFPS = Application.targetFrameRate;
        if (currentFPS == 30)
        {
            fps30Button.interactable = false;
        }
        else if (currentFPS == 60)
        {
            fps60Button.interactable = false;
        }
        else if (currentFPS == 120)
        {
            fps120Button.interactable = false;
        }
    }

    private void SetFPS(int fps)
    {
        Application.targetFrameRate = fps;

        // Reset button states
        fps30Button.interactable = true;
        fps60Button.interactable = true;
        fps120Button.interactable = true;

        // Disable the selected FPS button
        if (fps == 30)
        {
            fps30Button.interactable = false;
        }
        else if (fps == 60)
        {
            fps60Button.interactable = false;
        }
        else if (fps == 120)
        {
            fps120Button.interactable = false;
        }
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
        PlayerPrefs.SetInt("DisplayMode", (int)Screen.fullScreenMode);
        PlayerPrefs.SetInt("FPS", Application.targetFrameRate);
        PlayerPrefs.Save();
    }

    private void ApplySettings()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            AudioListener.volume = PlayerPrefs.GetFloat("Volume");
        }
        if (PlayerPrefs.HasKey("DisplayMode"))
        {
            SetDisplayMode(PlayerPrefs.GetInt("DisplayMode"));
        }
        if (PlayerPrefs.HasKey("FPS"))
        {
            Application.targetFrameRate = PlayerPrefs.GetInt("FPS");
        }
    }
}
