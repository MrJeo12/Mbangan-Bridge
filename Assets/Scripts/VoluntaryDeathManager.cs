using UnityEngine;
using UnityEngine.SceneManagement;

public class VoluntaryDeathManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject deathMenuPanel; //Reference to the voluntary death menu UI panel
    [SerializeField] private string mainMenuSceneName = "MainMenu"; //Name of the main menu scene to load when confirmed

    private bool menuActive = false; //Track whether the voluntary death menu is currently active

    private void Start()
    {
        //Make sure the voluntary death menu is hidden when the game first loads
        if (deathMenuPanel != null)
            deathMenuPanel.SetActive(false);
    }

    //Public method to display the voluntary death menu - called by specific death trigger zones
    public void ShowVoluntaryDeathMenu()
    {
        //Prevent opening the menu multiple times if it's already active
        if (menuActive) return;

        menuActive = true; //Mark the menu as now active

        //Display the death menu panel to the player - this is automatic when they enter the zone
        if (deathMenuPanel != null)
            deathMenuPanel.SetActive(true);

        //Freeze game time to pause all gameplay action
        Time.timeScale = 0f;

        //Disable player movement controls while menu is open
        EnablePlayerInput(false);

        Debug.Log("Voluntary death menu shown"); //Log for debugging
    }

    //Called when the player clicks the confirm button to return to main menu
    public void ConfirmReturnToMainMenu()
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
