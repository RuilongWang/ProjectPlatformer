using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Dialogue UI will handle how to draw out the text boxes when interacting with NPC characters
/// </summary>
public class DialogueUI : MonoBehaviour {

    #region main variables

    #endregion main variables

    #region monobehaviour methods
    private void Update()
    {

    }
    #endregion monobehaviour methods

    /// <summary>
    /// Initialize a dialogue action. Uses a dialoge interpreter to read in data for our 
    /// dialogue event
    /// </summary>
    public void InializeDialogue(DialogueInterpreter dialogueInterpreter)
    {

    }

    /// <summary>
    /// Clean up that we should use after a dialogue action has completed
    /// </summary>
    public void EndDialogue()
    {

    }


	
    /// <summary>
    /// Stores information  about a specific text box. 
    /// </summary>
    public class DialogueUINode
    {
        #region enums
        #endregion enums

        /// <summary>
        /// The text that will be displayed for this segment of dialogue
        /// </summary>
        public string textToDisplay;
        /// <summary>
        /// This is the image of the character that will be displayed
        /// </summary>
        public Image posingImageToDisplay;

        /// <summary>
        /// This is the connected dialouge node that will play naext after we complete this
        /// dialogue node. 
        /// 
        /// If this is null or has a size of zero the dialoge will end. If there are multiple options that a character can choose,
        /// we can pick the next UINode, based on the order of the option that was selected.
        /// </summary>
        public DialogueUINode[] nextDialogueNodesArray;
    }
}
