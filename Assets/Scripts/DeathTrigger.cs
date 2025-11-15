using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private Vector3 currentCheckpoint;  //Store checkpoint position directly instead of Transform reference

    private void Start()
    {
        //Auto set initial checkpoint to player's starting position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            currentCheckpoint = player.transform.position; //Store position, not transform reference
        }
    }

    //Method to update the checkpoint (called by Checkpoint script)
    public void UpdateCheckpoint(Transform newCheckpoint)
    {
        currentCheckpoint = newCheckpoint.position; //Store the position, not the transform
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the colliding object is the player
        if (collision.CompareTag("Player"))
        {
            //Teleport player back to checkpoint position
            collision.transform.position = currentCheckpoint;

            //Reset player velocity to prevent sliding after respawn
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
            }

            //Reset fear power if active (make sure PlayerFearController has this method)
            PlayerFearController fearController = collision.GetComponent<PlayerFearController>();
            if (fearController != null)
            {
                fearController.ForceDeactivateFear();
            }
        }
    }

    public Vector3 GetCurrentCheckpoint()
    {
        return currentCheckpoint; //Simply return the stored position
    }
}