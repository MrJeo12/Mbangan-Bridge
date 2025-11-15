using UnityEngine;
using System.Collections;

public class PlayerFearController : MonoBehaviour
{
    [SerializeField] private bool hasFearPower = false;  //Has the player unlocked fear power?
    [SerializeField] private float fearDuration = 5f;    //How long invisibility lasts
    [SerializeField] private float fearCooldown = 6f;    //Cooldown between uses
    [SerializeField] private GameObject fearEffect;      //Visual effect when using fear
    [SerializeField] private LayerMask enemyLayer;       //Layer for enemies - assign in Inspector

    private bool isFearActive = false;     //Is fear currently active?
    private bool isOnCooldown = false;     //Is fear on cooldown?
    private SpriteRenderer playerSprite;   //Reference to player's sprite renderer
    private Color originalColor;           //Store original player color
    private Collider2D playerCollider;     //Reference to player's collider

    private void Start()
    {
        //Get references to player components
        playerSprite = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();

        //Store original player color for later restoration
        if (playerSprite != null)
        {
            originalColor = playerSprite.color;
        }
    }

    private void Update()
    {
        //Check if player has fear power and presses E key to activate
        if (hasFearPower && Input.GetKeyDown(KeyCode.E) && !isOnCooldown && !isFearActive)
        {
            ActivateFear();
        }
    }

    public void UnlockFearPower()
    {
        hasFearPower = true;
        Debug.Log("Fear power unlocked! Press E to become invisible to enemies.");
    }

    private void ActivateFear()
    {
        isFearActive = true;

        //Make player transparent by reducing alpha
        if (playerSprite != null)
        {
            Color transparentColor = originalColor;
            transparentColor.a = 0.3f; //30% opacity
            playerSprite.color = transparentColor;
        }

        //Disable collision with enemies only (not ground!)
        if (playerCollider != null)
        {
            //Ignore collision with all objects on enemy layer
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Collider2D enemyCollider = enemy.GetComponent<Collider2D>();
                if (enemyCollider != null)
                {
                    Physics2D.IgnoreCollision(playerCollider, enemyCollider, true);
                }
            }
        }

        //Show visual effect
        if (fearEffect != null)
            fearEffect.SetActive(true);

        //Start timer to deactivate fear
        StartCoroutine(DeactivateFearAfterTime());

        Debug.Log("Fear activated! Player can pass through enemies but not ground.");
    }

    private IEnumerator DeactivateFearAfterTime()
    {
        yield return new WaitForSeconds(fearDuration);
        DeactivateFear();
        isOnCooldown = true;
        StartCoroutine(ResetCooldown());
    }

    private void DeactivateFear()
    {
        isFearActive = false;

        //Restore original player appearance
        if (playerSprite != null)
        {
            playerSprite.color = originalColor;
        }

        //Re-enable collision with enemies
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

        //Hide visual effect
        if (fearEffect != null)
            fearEffect.SetActive(false);

        Debug.Log("Fear deactivated. Cooldown started.");
    }

    private IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(fearCooldown);
        isOnCooldown = false;
        Debug.Log("Fear power ready! Press E to activate.");
    }

    public bool IsPlayerInvisible()
    {
        return isFearActive;
    }
}