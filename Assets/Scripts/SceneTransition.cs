using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string levelName;  //Name of the scene to load when player enters trigger

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the colliding object is the player
        if (collision.CompareTag("Player"))
        {
            //Load the specified scene when player enters the trigger
            SceneManager.LoadScene(levelName);
        }
    }
}
