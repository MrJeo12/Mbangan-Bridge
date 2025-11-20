using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pauseMenuPanel;    //Reference to the main pause menu UI panel
    [SerializeField] private GameObject settingsPanel;     //Reference to the settings UI panel within pause menu
    [SerializeField] private GameObject controlsPanel;     //Reference to the controls UI panel within pause menu

    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";  //Name of the main menu scene for returning

    private bool isPaused = false;  //Track whether the game is currently paused

    private void Start()
    {
        //Ensure the pause menu is completely hidden when the game first starts
        HideAllPanels();

        //Make sure the game is running at normal speed on start
        Time.timeScale = 1f;
    }

    private void Update()
    {
        //Check for Escape key press to toggle the pause menu on/off
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                //If already paused, resume the game
                ResumeGame();
            }
            else
            {
                //If not paused, pause the game
                PauseGame();
            }
        }
    }

    //Public method to resume the game - called by Resume button or Escape key
    public void ResumeGame()
    {
        isPaused = false;           //Mark game as unpaused
        Time.timeScale = 1f;        //Restore normal game time speed
        HideAllPanels();            //Hide all pause menu UI elements

        EnablePlayerInput(true); //Re-enable player controls so they can play again

        Debug.Log("Game resumed");  //Log for debugging purposes
    }

    //Private method to pause the game called internally by Escape key
    private void PauseGame()
    {
        isPaused = true;            //Mark game as paused
        Time.timeScale = 0f;        //Freeze game time to pause all action
        ShowPauseMenu();            //Display the pause menu UI

        EnablePlayerInput(false); //Disable player controls while game is paused

        Debug.Log("Game paused");   //Log for debugging purposes
    }

    //Show the main pause menu panel while hiding others
    public void ShowPauseMenu()
    {
        HideAllPanels();                //First hide all other panels
        pauseMenuPanel.SetActive(true); //Then show the main pause menu
    }

    //Show the settings panel while hiding others - called by Settings button
    public void ShowSettings()
    {
        HideAllPanels();            //Hide all panels including pause menu
        settingsPanel.SetActive(true); //Show the settings interface
    }

    //Show the controls panel while hiding others - called by Controls button
    public void ShowControls()
    {
        HideAllPanels();            //Hide all panels including pause menu
        controlsPanel.SetActive(true); //Show the controls information
    }

    //Return to the main menu - called by Main Menu button
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; //Crucial: reset time scale before scene change to prevent issues
        SceneManager.LoadScene(mainMenuSceneName); //Load the main menu scene

        Debug.Log("Returning to main menu"); //Log for debugging
    }

    //Helper method to hide all UI panels in the pause menu system
    private void HideAllPanels()
    {
        pauseMenuPanel.SetActive(false); //Hide main pause menu
        settingsPanel.SetActive(false);  //Hide settings panel
        controlsPanel.SetActive(false);  //Hide controls panel
    }

    //Public method to check current pause state
    public bool IsGamePaused()
    {
        return isPaused; //Return whether game is currently paused
    }

    private void EnablePlayerInput(bool enable)
    {
        //Search the scene for the player's movement script so we can control their input
        PlayerMovement player = FindObjectOfType<PlayerMovement>();

        //Make sure we actually found a player before trying to control them
        if (player != null)
        {
            //Tell the player whether they're allowed to move or not based on pause state
            player.SetCanMove(enable);
        }
    }
}