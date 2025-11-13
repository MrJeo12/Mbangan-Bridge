using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;        //Reference to the player's transform
    [SerializeField] private float smoothSpeed = 0.125f; //How smoothly the camera follows the player
    [SerializeField] private Vector3 offset;          //Camera position offset from player

    private void Update()
    {
        //Check if player reference exists to prevent errors
        if (player != null)
        {
            //Calculate the desired camera position based on player position and offset
            Vector3 desiredPosition = player.position + offset;

            //Smoothly interpolate between current camera position and desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            //Apply the smoothed position to the camera while maintaining Z position
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
    }
}
