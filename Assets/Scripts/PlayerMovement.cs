using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;   //Movement speed
    [SerializeField] private float jumpForce;   //Jump power
    private Rigidbody2D body;   //Reference to the physics component
    private bool grounded;  //Tracks if the player is touching the ground
    private bool onPlatform; //Tracks if player is on a platform

    private void Awake()
    {
        //Get the Rigidbody component attached to this GameObject
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");    //Get the horizontal input (-1 for left, 1 for right, 0 for no input)
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y); //Apply horizontal movement while preserving vertical velocity (gravity/jumping)

        //Jump when space is pressed AND player is grounded or on platform. Prevents infinite jumping mid air
        if (Input.GetKey(KeyCode.Space) && (grounded || onPlatform))
            Jump();
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);    //Apply upward velocity for jumping using jumpForce
        grounded = false;   //Player is no longer grounded
        onPlatform = false; //Player is no longer on platform
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if collision is with ground object
        if (collision.gameObject.tag == "Ground")
            grounded = true; //Player is now on solid ground and can jump again

        //Check if collision is with platform object  
        if (collision.gameObject.tag == "Platform")
            onPlatform = true; //Player is now on solid platform and can jump again
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //Continous check if player is touching ground or platform
        //This prevents losing jump ability during quick movements or running
        if (collision.gameObject.tag == "Ground")
            grounded = true; //Player is still on solid ground

        if (collision.gameObject.tag == "Platform")
            onPlatform = true; //Player is still on solid platform
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Only set to false if leaving the specific surface we were on
        if (collision.gameObject.tag == "Ground")
        {
            //Only set grounded to false if we're actually leaving ground
            //This prevents false negatives during quick movements
            grounded = false;
        }

        if (collision.gameObject.tag == "Platform")
        {
            onPlatform = false;
        }
    }
}