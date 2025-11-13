using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;        //Reference to the player's transform
    [SerializeField] private float smoothSpeed = 0.125f; //How smoothly the camera follows the player
    [SerializeField] private Vector3 offset;          //Camera position offset from player
    private float minYPosition;                       //Automatically detected ground level

    private void Start()
    {
        //Automatically find the lowest ground level when game starts
        FindLowestGroundLevel();
    }

    private void FindLowestGroundLevel()
    {
        //Find all GameObjects tagged as "Ground" in the scene
        GameObject[] groundObjects = GameObject.FindGameObjectsWithTag("Ground");

        if (groundObjects.Length > 0)
        {
            //Start with the first ground object's Y position
            minYPosition = groundObjects[0].transform.position.y;

            //Loop through all ground objects to find the lowest Y position
            foreach (GameObject ground in groundObjects)
            {
                if (ground.transform.position.y < minYPosition)
                    minYPosition = ground.transform.position.y;
            }

            //Add a small buffer (0.5 units) so camera stays clearly above ground
            minYPosition += 0.5f;

            Debug.Log("Auto-detected ground level: " + minYPosition);
        }
        else
        {
            //Fallback if no ground objects found
            minYPosition = 0f;
            Debug.LogWarning("No objects tagged 'Ground' found. Using default ground level.");
        }
    }

    private void Update()
    {
        //Check if player reference exists to prevent errors
        if (player != null)
        {
            //Calculate the desired camera position based on player position and offset
            Vector3 desiredPosition = player.position + offset;

            //Clamp the Y position to prevent camera from going below ground level
            float clampedY = Mathf.Max(desiredPosition.y, minYPosition);
            desiredPosition = new Vector3(desiredPosition.x, clampedY, desiredPosition.z);

            //Smoothly interpolate between current camera position and desired position
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            //Apply the smoothed position to the camera while maintaining Z position
            transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
        }
    }
}