using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the colliding object is the player
        if (collision.CompareTag("Player"))
        {
            //Find the DeathTrigger in the scene and update its checkpoint
            DeathTrigger deathTrigger = FindObjectOfType<DeathTrigger>();
            if (deathTrigger != null)
            {
                //Set this checkpoint as the new respawn point
                deathTrigger.UpdateCheckpoint(transform);
            }
        }
    }
}