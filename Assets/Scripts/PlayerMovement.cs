using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;   //Movement speed and jump power
    [SerializeField] private float wallSlideDetection = 0.1f; //Distance to check for walls in front of player

    private Rigidbody2D body;   //Reference to the physics component
    private bool grounded;  //Tracks if the player is touching the ground
    private bool onPlatform; //Tracks if player is on a platform
    private bool isTouchingWall; //Tracks if player is touching a wall while in air

    private void Awake()
    {
        //Get the Rigidbody component attached to this GameObject
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");    //Get the horizontal input (-1 for left, 1 for right, 0 for no input)

        //Check for walls before applying movement
        CheckWallContact(horizontalInput);

        //Apply movement with wall prevention
        if (isTouchingWall && !grounded && !onPlatform)
        {
            //When touching wall in air, allow sliding down but no horizontal push against wall
            body.linearVelocity = new Vector2(0, body.linearVelocity.y);
        }
        else
        {
            //Normal movement with full air control - apply horizontal movement while preserving vertical velocity
            body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y);
        }

        //Jump when space is pressed AND player is grounded or on platform. Prevents infinite jumping mid air
        if (Input.GetKey(KeyCode.Space) && (grounded || onPlatform))
            Jump();
    }

    private void CheckWallContact(float horizontalInput)
    {
        //Only check for walls if we're moving toward them AND in air
        if (horizontalInput != 0 && !grounded && !onPlatform)
        {
            //Cast a ray in the direction we're moving to detect walls
            Vector2 direction = new Vector2(horizontalInput, 0);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallSlideDetection);

            //Check if we hit a wall (ground or platform)
            isTouchingWall = hit.collider != null &&
                           (hit.collider.tag == "Ground" || hit.collider.tag == "Platform");
        }
        else
        {
            //Not touching wall if not moving or on ground
            isTouchingWall = false;
        }
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, speed);    //Apply upward velocity for jumping using speed value
        grounded = false;   //Player is no longer grounded
        onPlatform = false; //Player is no longer on platform
        isTouchingWall = false; //Jumping away from wall
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if collision is with ground object
        if (collision.gameObject.tag == "Ground")
            grounded = true; //Player is now on solid ground and can jump again

        //Check if collision is with platform object  
        if (collision.gameObject.tag == "Platform")
            onPlatform = true; //Player is now on platform and can jump again
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Check if collision is with ground object
        if (collision.gameObject.tag == "Ground")
            grounded = false; //Player is no longer touching ground, cannot jump

        //Check if collision is with platform object
        if (collision.gameObject.tag == "Platform")
            onPlatform = false; //Player is no longer touching platform, cannot jump
    }

}