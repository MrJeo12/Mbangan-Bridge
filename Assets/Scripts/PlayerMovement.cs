using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;   //Movement speed
    private Rigidbody2D body;   //Reference to the physics component
    private bool grounded;  //Tracks if the player is touching the ground
    private bool onPlatform; //Tracks if player is on a platform

    private void Awake()
    {
        //Get the Rigidbody component attactched to this GameObject
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");    //Get the horizontal input (-1 for left, 1 for right, 0 for no input)
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y); //Apply horizontal movement while preserving vertical velocity (gravity/jumping)


        // Jump when space is pressed AND player is grounded or on platform. Prevents infinite jumping mid air
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            Jump();

        } else if (Input.GetKey(KeyCode.Space) && onPlatform)
            Jump();

    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, speed);    //Apply upward velocity for jumping
        grounded = false;   //Player is no longer grounded
        onPlatform = false; //Player is no longer on platform
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if collision is with ground object
        if (collision.gameObject.tag == "Ground")
            grounded = true; // Player is now on solid ground and can jump again
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Check if collision is with ground object
        if (collision.gameObject.tag == "Ground")
            grounded = false; // Player is no longer touching ground, cannot jump
    }
}