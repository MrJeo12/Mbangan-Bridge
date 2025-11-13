using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;  // Reference to the previous room's transform
    [SerializeField] private Transform nextRoom;      // Reference to the next room's transform
    [SerializeField] private CameraController cam;    // Reference to the camera controller

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player
        if (collision.tag == "Player")
        {
            // Determine which direction the player is coming from and move camera accordingly
            if (collision.transform.position.x < transform.position.x)
                cam.MoveToNewRoom(nextRoom);      // Player is coming from left, move to next room
            else
                cam.MoveToNewRoom(previousRoom);  // Player is coming from right, move to previous room
        }
    }
}