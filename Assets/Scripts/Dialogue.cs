using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class Dialogue : MonoBehaviour
{
    //Fields
    public GameObject window; //Window
    public GameObject indicator; //Indicator
    public TMP_Text dialogueText; //Text Component
    public List<string> dialogues; //Dialogue list
    public float writingSpeed; //Writing speed
    private int index; //Index on dialogue
    private int charIndex; //Character index
    private bool started; //Started boolean
    private bool waitForNext; //Wait for next boolean


    private void ToggleWindow(bool show)
    {
        window.SetActive(show);
    }

    public void ToggleIndicator(bool show)
    {
        indicator.SetActive(show);
    }

    //Start Dialogue
    public void StartDialogue()
    {
        if (started)
            return;

        started = true; //Boolean to indicate that dialogue has started

        ToggleWindow(true); //Show window

        ToggleIndicator(false); //Hide the indicator

        GetDialogue(0); //Start with first dialogue
    }

    private void GetDialogue(int i)
    {
        index = i; //Start index at 0
        charIndex = 0; //Reset the character index

        dialogueText.text = ""; //Clear the dialogue component text

        StartCoroutine(Writing()); //Start writting
    }

    //End Dialogue
    public void EndDialogue()
    {
        ToggleWindow(false); //Hide the window
    }

    //Writting logic
    IEnumerator Writing()
    {
        string currentDialogue = dialogues[index];
        dialogueText.text += currentDialogue[charIndex]; //Write the character
        charIndex++;//Increase the character index

        //Make sure end of the sentence has been reached
        if (charIndex < currentDialogue.Length)
        {
            yield return new WaitForSeconds(writingSpeed);//Wait for x seconds
            StartCoroutine(Writing());//Restart the same process
        }
        else
        {
            waitForNext = true;
        }

    }

    private void Update()
    {

        if (!started)
            return;

        if (waitForNext && Input.GetKeyDown(KeyCode.F))
        {
            waitForNext = false;
            index++;

            if (index < dialogues.Count)
            {
                GetDialogue(index);
            }
            else
            {
                EndDialogue();
            }

        }
    }
}

