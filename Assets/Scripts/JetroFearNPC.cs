using UnityEngine;

public class JetroFearNPC : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;  //Reference to dialogue UI
    [SerializeField] private string dialogueText = "I grant you the power of FEAR! Press E to become invisible to enemies for 5 seconds.";

    private bool playerInRange = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;

            //Show dialogue
            if (dialogueBox != null)
                dialogueBox.SetActive(true);

            Debug.Log(dialogueText);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;

            //Hide dialogue
            if (dialogueBox != null)
                dialogueBox.SetActive(false);
        }
    }

    private void Update()
    {
        //If player is in range and presses interact key (E key example)
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
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
            Debug.Log("Fear power granted!");

            //Hide dialogue after granting power
            if (dialogueBox != null)
                dialogueBox.SetActive(false);

            //Disable NPC after granting power
            this.enabled = false;
        }
    }
}
