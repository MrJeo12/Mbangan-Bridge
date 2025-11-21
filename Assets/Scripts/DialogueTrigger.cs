using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogueScript;
    private bool playerDetected;
    private bool hasTriggered = false; //Add this flag to track if already triggered

    //Detect trigger with player
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //If we triggered the player AND haven't triggered before, enable playerdetected and show indicator
        if (collision.tag == "Player" && !hasTriggered)
        {
            playerDetected = true;
            dialogueScript.ToggleIndicator(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //If we lost trigger with the player disable playerdetected and hide indicator
        if (collision.tag == "Player")
        {
            playerDetected = false;
            dialogueScript.ToggleIndicator(false);
            dialogueScript.EndDialogue();
        }
    }

    //While detected if we interact start the dialogue
    private void Update()
    {
        if (playerDetected && Input.GetKeyDown(KeyCode.F) && !hasTriggered)
        {
            dialogueScript.StartDialogue();
            hasTriggered = true; // Mark as triggered so it can't happen again
            playerDetected = false; // Immediately disable detection
            dialogueScript.ToggleIndicator(false); // Hide the indicator immediately
        }
    }
}