using UnityEngine;

public class JetroFearNPC : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;  //Reference to dialogue UI - assign in Inspector

    private bool playerInRange = false;    //Tracks if player is inside the NPC's trigger zone
    private bool powerGranted = false;     //Prevents granting power multiple times

    //Called when another collider enters this NPC's trigger zone
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the entering collider is the player AND power hasn't been granted yet
        if (collision.CompareTag("Player") && !powerGranted)
        {
            playerInRange = true;  //Player is now within interaction range
            Debug.Log("Player entered NPC trigger zone");

            //Show dialogue box to indicate player can interact
            if (dialogueBox != null)
                dialogueBox.SetActive(true);
        }
    }

    //Called when another collider exits this NPC's trigger zone
    private void OnTriggerExit2D(Collider2D collision)
    {
        //Check if the exiting collider is the player
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;  //Player is no longer in interaction range
            Debug.Log("Player left NPC trigger zone");

            //Hide dialogue box since player moved away
            if (dialogueBox != null)
                dialogueBox.SetActive(false);
        }
    }

    private void Update()
    {
        //Check every frame if player is in range, presses E key, and power hasn't been granted
        //This allows for responsive interaction without needing to click on the NPC
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !powerGranted)
        {
            GrantFearPower();
        }
    }

    //Main method that gives the fear power ability to the player
    private void GrantFearPower()
    {
        //Search the scene for the PlayerFearController component on the player
        PlayerFearController playerFear = FindObjectOfType<PlayerFearController>();

        //Verify the player fear controller was found before proceeding
        if (playerFear != null)
        {
            //Call the method that enables fear power on the player
            playerFear.UnlockFearPower();
            powerGranted = true;  //Mark power as granted to prevent duplicate grants
            Debug.Log("Fear power granted to player!");

            //Hide dialogue box since interaction is complete
            if (dialogueBox != null)
                dialogueBox.SetActive(false);

            //This makes the NPC non-interactable after giving the power
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            //Error handling - helps debug if something goes wrong
            Debug.LogError("PlayerFearController not found in scene!");
        }
    }
}