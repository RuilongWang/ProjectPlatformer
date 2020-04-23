using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogueReference;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TriggerDialogue();
        }
    }


    public void TriggerDialogue()
    {
        InGameHUD.Instance.DialogueManagerComponent.DialogueSequenceBegin(dialogueReference);
    }
}
