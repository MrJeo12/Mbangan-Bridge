using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;   //Movement speed
    private Rigidbody2D body;   //Reference to the physics component
    private bool grounded;  //Tracks if the player is touching the ground

    private void Awake()
    {
        //Get the Rigidbody component attactched to this GameObject
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");    //Get the horizontal input (-1 for left, 1 for right, 0 for no input)
                                                                //
        // Only apply horizontal movement when grounded
        if (grounded)
        {
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y); //Apply horizontal movement while preserving vertical velocity (gravity/jumping)

        }


        // Jump when space is pressed AND player is grounded
        if (Input.GetKey(KeyCode.Space) && grounded)
            Jump();

    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, speed);    //Apply upward velocity for jumping
        grounded = false;   //Player is no longer grounded
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if collision is with ground object
        if (collision.gameObject.tag == "Ground")
            grounded = true;
    }
}