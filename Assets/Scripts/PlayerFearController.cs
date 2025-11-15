using UnityEngine;
using System.Collections;

public class PlayerFearController : MonoBehaviour
{
    [SerializeField] private bool hasFearPower = false;  //Has the player unlocked fear power?
    [SerializeField] private float fearDuration = 5f;    //How long invisibility lasts
    [SerializeField] private float fearCooldown = 6f;    //Cooldown between uses
    [SerializeField] private GameObject fearEffect;      //Visual effect when using fear

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
            transparentColor.a = 0.3f; //30% opacity (adjust as needed)
            playerSprite.color = transparentColor;
        }

        //Disable player collider to pass through enemies
        if (playerCollider != null)
        {
            playerCollider.enabled = false;
        }

        //Show visual effect (particles, etc.)
        if (fearEffect != null)
            fearEffect.SetActive(true);

        //Start timer to deactivate fear
        StartCoroutine(DeactivateFearAfterTime());

        Debug.Log("Fear activated! Player is invisible and can pass through enemies.");
    }

    private IEnumerator DeactivateFearAfterTime()
    {
        //Wait for fear duration to complete
        yield return new WaitForSeconds(fearDuration);

        //Deactivate fear and restore normal state
        DeactivateFear();

        //Start cooldown period
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

        //Re-enable player collider
        if (playerCollider != null)
        {
            playerCollider.enabled = true;
        }

        //Hide visual effect
        if (fearEffect != null)
            fearEffect.SetActive(false);

        Debug.Log("Fear deactivated. Cooldown started.");
    }

    private IEnumerator ResetCooldown()
    {
        //Wait for cooldown period
        yield return new WaitForSeconds(fearCooldown);
        isOnCooldown = false;
        Debug.Log("Fear power ready! Press E to activate.");
    }

    //Public method for enemies to check if player is invisible
    public bool IsPlayerInvisible()
    {
        return isFearActive;
    }
}