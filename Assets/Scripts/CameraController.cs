using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float speed;        // How quickly the camera moves between rooms (lower = slower)
    private float currentPosX;                   // Target X position the camera is moving towards
    private Vector3 velocity = Vector3.zero;     // Reference velocity used by SmoothDamp for smooth movement

    private void Update()
    {
        // Smoothly move camera to target X position while keeping current Y and Z positions
        // SmoothDamp creates smooth easing movement instead of instant snapping
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(currentPosX, transform.position.y, transform.position.z), ref velocity, speed);
    }

    public void MoveToNewRoom(Transform _newRoom)
    {
        // Set new target X position based on the new room's position
        // This will make the camera smoothly move to focus on the new room
        currentPosX = _newRoom.position.x;
    }
}

