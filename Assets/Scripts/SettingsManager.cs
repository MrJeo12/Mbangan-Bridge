using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [Header("Settings UI")]
    [SerializeField] private Slider volumeSlider;        //Reference to the volume slider UI element
    [SerializeField] private TMP_Dropdown qualityDropdown; //Reference to the graphics quality dropdown UI element

    private void Start()
    {
        //Initialize the volume slider with the current audio volume setting
        volumeSlider.value = AudioListener.volume;

        //Set the quality dropdown to display the current graphics quality level
        qualityDropdown.value = QualitySettings.GetQualityLevel();

        //Add event listeners to detect when the user changes settings
        volumeSlider.onValueChanged.AddListener(SetVolume);
        qualityDropdown.onValueChanged.AddListener(SetQuality);

        //Log the current quality level for debugging purposes
        Debug.Log("Current quality level: " + QualitySettings.GetQualityLevel());
    }

    //Method called when the volume slider value changes
    private void SetVolume(float volume)
    {
        //Apply the new volume level to the game's audio
        AudioListener.volume = volume;

        //Save the volume setting to persistent storage for future game sessions
        PlayerPrefs.SetFloat("MasterVolume", volume);
        PlayerPrefs.Save();
    }

    //Method called when the quality dropdown selection changes
    private void SetQuality(int qualityIndex)
    {
        //Apply the selected graphics quality level to the game
        QualitySettings.SetQualityLevel(qualityIndex);

        //Save the quality setting to persistent storage for future game sessions
        PlayerPrefs.SetInt("QualitySetting", qualityIndex);
        PlayerPrefs.Save();

        //Log the new quality setting for debugging and confirmation
        Debug.Log("Quality set to level: " + qualityIndex + " - " + QualitySettings.names[qualityIndex]);
    }

    //Public method to load previously saved settings when the game starts
    public void LoadSavedSettings()
    {
        //Check if a saved volume setting exists and load it
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume");
            AudioListener.volume = savedVolume;
            volumeSlider.value = savedVolume;
        }

        //Check if a saved quality setting exists and load it
        if (PlayerPrefs.HasKey("QualitySetting"))
        {
            int savedQuality = PlayerPrefs.GetInt("QualitySetting");
            QualitySettings.SetQualityLevel(savedQuality);
            qualityDropdown.value = savedQuality;
        }
    }
}
