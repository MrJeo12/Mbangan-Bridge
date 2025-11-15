using UnityEngine;

public class EnemyGuard : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;  //Array of patrol positions - assign in Inspector
    [SerializeField] private float moveSpeed = 2f;      //How fast enemy moves between patrol points
    [SerializeField] private float detectionRange = 3f; //How far enemy can see/spot the player
    [SerializeField] private LayerMask obstacleLayer;   //Layer mask for walls/obstacles that block vision

    private int currentPatrolIndex = 0;                 //Current target patrol point index
    private Transform player;                           //Reference to player's transform
    private DeathTrigger deathTrigger;                  //Reference to death/respawn system

    private void Start()
    {
        //Find player object by tag and store reference
        player = GameObject.FindGameObjectWithTag("Player").transform;

        //Find the death trigger system in the scene
        deathTrigger = FindObjectOfType<DeathTrigger>();
    }

    private void Update()
    {
        //Ensure player reference exists before running logic
        if (player != null)
        {
            //Check if player is detected AND not invisible from fear power
            if (IsPlayerDetected() && !IsPlayerInvisible())
            {
                PlayerDetected();  //Player spotted - trigger respawn
            }
            else
            {
                Patrol();  //No player detected - continue normal patrol
            }
        }
    }

    private void Patrol()
    {
        //Only patrol if patrol points are assigned
        if (patrolPoints.Length > 0)
        {
            //Get current target patrol point
            Transform targetPoint = patrolPoints[currentPatrolIndex];

            //Move enemy towards target patrol point at specified speed
            transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

            //Check if enemy has reached the current patrol point
            if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
            {
                //Move to next patrol point (loop back to 0 after last point)
                currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            }
        }
    }

    private bool IsPlayerDetected()
    {
        //Calculate distance to player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        //Check if player is within detection range
        if (distanceToPlayer <= detectionRange)
        {
            //Cast ray towards player to check for obstacles/walls
            Vector2 directionToPlayer = player.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, detectionRange, obstacleLayer);

            //Player is detected if no obstacles OR if the obstacle is the player themselves
            return hit.collider == null || hit.collider.CompareTag("Player");
        }
        return false; //Player too far away to detect
    }

    private bool IsPlayerInvisible()
    {
        //Check if player has fear power component and is currently invisible
        PlayerFearController playerFear = player.GetComponent<PlayerFearController>();
        return playerFear != null && playerFear.IsPlayerInvisible();
    }

    private void PlayerDetected()
    {
        Debug.Log("Player detected! Sending back to checkpoint.");

        //Trigger death/respawn sequence
        if (deathTrigger != null)
        {
            //Teleport player to last checkpoint position
            player.position = deathTrigger.GetCurrentCheckpoint();

            //Reset player's movement velocity to prevent sliding
            Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
            }
        }
    }

    //Visualize detection range in Scene view for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
