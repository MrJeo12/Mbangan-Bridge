using UnityEngine;
using System.Collections;

public class PlayerFearController : MonoBehaviour
{
    [SerializeField] private bool hasFearPower = false;  //Has the player unlocked fear power? Set by NPC
    [SerializeField] private float fearDuration = 5f;    //How long invisibility lasts in seconds
    [SerializeField] private float fearCooldown = 6f;    //Cooldown between uses in seconds

    private bool isFearActive = false;     //Is fear currently active? Used for enemy detection
    private bool isOnCooldown = false;     //Is fear on cooldown? Prevents spamming
    private SpriteRenderer playerSprite;   //Reference to player's sprite renderer for transparency effect
    private Color originalColor;           //Store original player color to restore later
    private Collider2D playerCollider;     //Reference to player's collider for physics interactions

    private void Start()
    {
        //Get references to player components needed for fear power effects
        playerSprite = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();

        //Store original player color for later restoration when fear ends
        if (playerSprite != null)
        {
            originalColor = playerSprite.color;
        }
    }

    private void Update()
    {
        //Check if player has fear power and presses E key to activate
        //Also verify fear is not on cooldown and not already active
        if (hasFearPower && Input.GetKeyDown(KeyCode.E) && !isOnCooldown && !isFearActive)
        {
            ActivateFear();
        }
    }

    //Public method called by NPC to grant fear power to player
    public void UnlockFearPower()
    {
        hasFearPower = true;
        Debug.Log("Fear power unlocked! Press E to become invisible to enemies.");
    }

    //Main method that activates all fear power effects
    private void ActivateFear()
    {
        isFearActive = true;

        //Make player transparent by reducing alpha - visual feedback for player
        if (playerSprite != null)
        {
            Color transparentColor = originalColor;
            transparentColor.a = 0.3f; //30% opacity - makes player semi-transparent
            playerSprite.color = transparentColor;
        }

        //Disable collision with enemies only (not ground!) - allows phasing through enemies
        if (playerCollider != null)
        {
            //Find all enemy objects in the scene by tag
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                //Get each enemy's collider and ignore collisions with player
                Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
                if (enemyCollider != null)
                {
                    Physics2D.IgnoreCollision(playerCollider, enemyCollider, true);
                }
            }
        }

        //Start timer to automatically deactivate fear after duration ends
        StartCoroutine(DeactivateFearAfterTime());

        Debug.Log("Fear activated! Player becomes undetectable and can pass through enemies!");
    }

    //Coroutine that waits for fear duration then deactivates power
    private IEnumerator DeactivateFearAfterTime()
    {
        //Wait for the specified fear duration before continuing
        yield return new WaitForSeconds(fearDuration);

        //Deactivate fear effects and restore normal state
        DeactivateFear();

        //Start cooldown period where fear cannot be used
        isOnCooldown = true;
        StartCoroutine(ResetCooldown());
    }

    //Method to deactivate all fear power effects and restore normal state
    private void DeactivateFear()
    {
        isFearActive = false;

        //Restore original player appearance - remove transparency
        if (playerSprite != null)
        {
            playerSprite.color = originalColor;
        }

        //Re-enable collision with enemies - player can be detected and blocked again
        if (playerCollider != null)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
                if (enemyCollider != null)
                {
                    Physics2D.IgnoreCollision(playerCollider, enemyCollider, false);
                }
            }
        }

        Debug.Log("Fear deactivated. Cooldown started.");
    }

    //Method to deactivate fear power when player enters death trigger/gets sent back to checkpoint 
    public void ForceDeactivateFear()
    {
        if (isFearActive)
        {
            // Immediately deactivate fear without starting cooldown
            isFearActive = false;
            isOnCooldown = false; // Reset cooldown so player can use fear again immediately

            // Restore player appearance
            if (playerSprite != null)
            {
                playerSprite.color = originalColor;
            }

            // Re-enable enemy collisions
            if (playerCollider != null)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in enemies)
                {
                    Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
                    if (enemyCollider != null)
                    {
                        Physics2D.IgnoreCollision(playerCollider, enemyCollider, false);
                    }
                }
            }

            Debug.Log("Fear power force-deactivated due to death");
        }
    }

    //Coroutine that waits for cooldown period then makes fear available again
    private IEnumerator ResetCooldown()
    {
        //Wait for the specified cooldown period before continuing
        yield return new WaitForSeconds(fearCooldown);

        //Reset cooldown flag - fear power can now be used again
        isOnCooldown = false;
        Debug.Log("Fear power ready! Press E to activate.");
    }

    //Public method for enemies to check if player is currently invisible/using fear
    //EnemyGuard script calls this to determine if they should detect player
    public bool IsPlayerInvisible()
    {
        return isFearActive;
    }
}