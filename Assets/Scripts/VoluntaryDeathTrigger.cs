using UnityEngine;

public class VoluntaryDeathTrigger : MonoBehaviour
{
    private VoluntaryDeathManager deathManager; //Reference to the death menu controller

    private void Start()
    {
        //Locate the death menu manager in the scene so we can communicate with it
        deathManager = FindObjectOfType<VoluntaryDeathManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the colliding object is the player AND we successfully found the death manager
        if (collision.CompareTag("Player") && deathManager != null)
        {
            //Trigger the death menu to appear - player has entered the point of no return
            deathManager.ShowVoluntaryDeathMenu();

            Debug.Log("Player entered voluntary death zone"); //Log for debugging purposes
        }
    }
}