using UnityEngine;

public class JetroFearNPC : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;  //Reference to dialogue UI

    private bool playerInRange = false;
    private bool powerGranted = false;  //Prevent granting power multiple times

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !powerGranted)
        {
            playerInRange = true;
            Debug.Log("Player entered NPC trigger zone");

            //Show dialogue
            if (dialogueBox != null)
                dialogueBox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left NPC trigger zone");

            //Hide dialogue
            if (dialogueBox != null)
                dialogueBox.SetActive(false);
        }
    }

    private void Update()
    {
        //If player is in range and presses interact key (E key)
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !powerGranted)
        {
            GrantFearPower();
        }
    }

    private void GrantFearPower()
    {
        //Find player and grant fear power
        PlayerFearController playerFear = FindObjectOfType<PlayerFearController>();
        if (playerFear != null)
        {
            playerFear.UnlockFearPower();
            powerGranted = true;
            Debug.Log("Fear power granted to player!");

            //Hide dialogue after granting power
            if (dialogueBox != null)
                dialogueBox.SetActive(false);

            //Optional: Disable NPC collider so player can't interact again
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            Debug.LogError("PlayerFearController not found in scene!");
        }
    }
}