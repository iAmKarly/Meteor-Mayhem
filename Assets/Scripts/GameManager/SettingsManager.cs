using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider volumeSlider;
    public Button fullScreenButton;
    public Button windowedButton;
    public Button fps30Button;
    public Button fps60Button;
    public Button fps120Button;
    public Button applyButton;

    private float initialVolume;
    private UnityEngine.FullScreenMode initialDisplay;
    private int initialFPS;
    private float currentVolume;
    private UnityEngine.FullScreenMode currentDisplay;
    private int currentFPS;

    private bool saved = false;

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
        applyButton.onClick.AddListener(ApplySettings);
    }

    /// <summary>
    /// Updates the volume slider based on the current volume
    /// </summary>
    private void InitializeVolume()
    {
        volumeSlider.value = AudioListener.volume;
        initialVolume = AudioListener.volume;
    }

    /// <summary>
    /// Updates volume
    /// </summary>
    /// <param name="volume">The new volume</param>
    private void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        currentVolume = volume;
        saved = false;
    }

    /// <summary>
    /// Updates the display mode buttons based on the current display mode
    /// </summary>
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

        initialDisplay = Screen.fullScreenMode;
    }

    /// <summary>
    /// Updates display mode
    /// </summary>
    /// <param name="modeIndex">The new display mode</param>
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
        currentDisplay = mode;
        Screen.fullScreenMode = mode;
        Debug.Log($"Set display mode to: {Screen.fullScreenMode}");
        InitializeDisplayModeButtons(); // Update button states
        saved = false;;
    }

    /// <summary>
    /// Updates the fps buttons based on the current fps
    /// </summary>
    private void InitializeFPS()
    {
        initialFPS = Application.targetFrameRate;
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

    /// <summary>
    /// Sets the fps
    /// </summary>
    /// <param name="fps">The new fps</param>
    private void SetFPS(int fps)
    {
        Application.targetFrameRate = fps;
        currentFPS = fps;

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
        saved = false;
    }

    /// <summary>
    /// Applies the settings so its not reverted
    /// </summary>
    private void ApplySettings()
    {
        PlayerPrefs.SetFloat("Volume", currentVolume);
        PlayerPrefs.SetInt("DisplayMode", (int)Screen.fullScreenMode);
        PlayerPrefs.SetInt("FPS", Application.targetFrameRate);
        PlayerPrefs.Save();

        initialVolume = currentVolume;

        initialDisplay = currentDisplay;

        initialFPS = currentFPS;
        
        saved = true;
    }

    /// <summary>
    /// Load the scene named "Home Screen" and revert changes if not saved
    /// </summary>
    public void OpenHome()
    {
        SceneManager.LoadScene("Home Screen");
        if(!saved){
            AudioListener.volume = initialVolume;
            Screen.fullScreenMode = initialDisplay;
            Application.targetFrameRate = initialFPS;
        }
    }
}
