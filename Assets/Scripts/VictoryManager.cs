using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject victoryMenuPanel; //Reference to the victory menu UI panel
    [SerializeField] private string mainMenuSceneName = "MainMenu"; //Name of the main menu scene to load

    private bool menuActive = false; //Track whether the victory menu is currently active

    private void Start()
    {
        //Make sure the victory menu is hidden when the game first loads
        if (victoryMenuPanel != null)
            victoryMenuPanel.SetActive(false);
    }

    //Public method to display the victory menu - called by victory trigger zones
    public void ShowVictoryMenu()
    {
        //Prevent opening the menu multiple times if it's already active
        if (menuActive) return;

        menuActive = true; //Mark the menu as now active

        //Display the victory menu panel to the player
        if (victoryMenuPanel != null)
            victoryMenuPanel.SetActive(true);

        //Freeze game time to pause all gameplay action
        Time.timeScale = 0f;

        //Disable player movement controls while menu is open
        EnablePlayerInput(false);

        Debug.Log("Victory menu shown"); //Log for debugging
    }

    //Called when the player clicks the button to return to main menu
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f; //Restore normal time scale before scene change
        SceneManager.LoadScene(mainMenuSceneName); //Load the main menu scene
    }

    //Helper method to enable or disable player movement input
    private void EnablePlayerInput(bool enable)
    {
        //Find the player's movement script in the scene
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            //Enable or disable movement based on the provided parameter
            player.SetCanMove(enable);
        }
    }
}