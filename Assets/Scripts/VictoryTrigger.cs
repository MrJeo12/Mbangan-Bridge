using UnityEngine;

public class VictoryTrigger : MonoBehaviour
{
    private VictoryManager victoryManager; //Reference to the victory menu controller

    private void Start()
    {
        //Locate the victory manager in the scene so we can communicate with it
        victoryManager = FindObjectOfType<VictoryManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the colliding object is the player AND we successfully found the victory manager
        if (collision.CompareTag("Player") && victoryManager != null)
        {
            //Trigger the victory menu to appear - player has successfully completed the level
            victoryManager.ShowVictoryMenu();

            Debug.Log("Player entered victory zone"); //Log for debugging purposes
        }
    }
}