using UnityEngine;

public class DylanDialogue : MonoBehaviour
{
    //Fields
    public GameObject window; //Window
    public GameObject indicator; //Indicator
    public List<string> dialogues; //Dialogue list
    private int index; //Index on dialogue
    private int charIndex; //Character index

    //Start Dialogue
    public void StartDialogue()
    {
        //Show window
        //Hide the indicator
        //Start index at 0
        //Start writting
    }

    //End Dialogue
    public void EndDialogue()
    {
        //Hide the window
    }

    //Writting logic
    IEnumerator Writting()
    {
        //Write the character
        //Increase the character index
        //Wait for x seconds
        //Restart the same process
    }
}
