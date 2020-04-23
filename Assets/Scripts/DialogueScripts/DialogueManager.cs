using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public Text dialogueBoxText;


    private Queue<string> sentenceQueue;
    #region monobehaviour methods

    private void Awake()
    {
        sentenceQueue = new Queue<string>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }
    #endregion monobehaviour methods



    /// <summary>
    /// Beigins a dialogue sequence that the player can interact with
    /// </summary>
    /// <param name="dialogue"></param>
    public void DialogueSequenceBegin(Dialogue dialogue)
    {
        this.gameObject.SetActive(true);
        Debug.Log("Starting conversation with " + dialogue.characterName);
        sentenceQueue.Clear();

        foreach (string sentence in dialogue.sentenceList)
        {
            sentenceQueue.Enqueue(sentence);
        }

        DisplayNextSentence();//Starts the sequence of dialogue
        
    }

    /// <summary>
    /// This will display the next sentence in our queue. This will end the dialogue sequence if there
    /// is nothitng left to display
    /// </summary>
    public void DisplayNextSentence()
    {
        if (sentenceQueue.Count == 0)
        {
            DialogueSequenceEnd();
            return;
        }

        string nextSentenceToDisplay = sentenceQueue.Dequeue();
        dialogueBoxText.text = nextSentenceToDisplay;
    }

    /// <summary>
    /// This method will be called at the end of a dialogue sequence. Any cleanup that is necessary
    /// should be taken care of here
    /// </summary>
    private void DialogueSequenceEnd()
    {
        Debug.Log("Sentence Ended");
        this.gameObject.SetActive(false);
    }
}
