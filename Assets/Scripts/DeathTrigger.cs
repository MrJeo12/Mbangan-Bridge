using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private Transform currentCheckpoint;  //Current active checkpoint

    //Method to update the checkpoint (called by Checkpoint script)
    public void UpdateCheckpoint(Transform newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
    }

    private void OnTriggerEnter2D(Collider2D collision)  //Changed to Collider2D
    {
        //Check if the colliding object is the player AND we have a checkpoint
        if (collision.CompareTag("Player") && currentCheckpoint != null)
        {
            //Teleport player back to checkpoint
            collision.transform.position = currentCheckpoint.position;

            //Reset player velocity - use collision.gameObject
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
            }
        }
    }
}