using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;    //Reference to the main menu UI panel
    [SerializeField] private GameObject settingsPanel;    //Reference to the settings UI panel
    [SerializeField] private GameObject controlsPanel;    //Reference to the controls UI panel

    [Header("Scene Names")]
    [SerializeField] private string firstLevelSceneName = "Level1";  //Name of the first level scene to load

    private void Start()
    {
        //Ensure only the main menu is visible when the game starts
        ShowMainMenu();

        //Reset time scale to normal in case we're returning from a paused game state
        Time.timeScale = 1f;
    }

    //Called when the Start Game button is clicked - begins the gameplay
    public void StartGame()
    {
        Debug.Log("Starting game...");

        //Load the first level scene to begin the game
        SceneManager.LoadScene(firstLevelSceneName);
    }

    //Called when the Settings button is clicked - shows the settings menu
    public void ShowSettings()
    {
        //Hide the main menu and show the settings panel
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false); //Ensure controls panel is hidden if it was open
        settingsPanel.SetActive(true);  //Show the settings panel
    }

    //Called when the Controls button is clicked - shows the controls information
    public void ShowControls()
    {
        //Hide the main menu and show the controls panel
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(false); //Ensure settings panel is hidden if it was open
        controlsPanel.SetActive(true);  //Show the controls panel
    }

    //Called when any Back button is clicked - returns to the main menu from other panels
    public void ShowMainMenu()
    {
        //Hide all other panels and show the main menu
        settingsPanel.SetActive(false);   //Hide settings panel
        controlsPanel.SetActive(false);   //Hide controls panel
        mainMenuPanel.SetActive(true);    //Show main menu panel
    }

    //Called when the Quit button is clicked - exits the game
    public void QuitGame()
    {
        Debug.Log("Quitting game...");

        //Handle quitting in both editor and built game
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  //Stop play mode in editor
        #else
        Application.Quit();  //Close the application in built game
        #endif
    }
}
