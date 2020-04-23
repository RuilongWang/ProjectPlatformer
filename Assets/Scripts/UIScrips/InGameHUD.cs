using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the overseeing UI for our in game UI component. This class will contain references to other
/// components of our HUD for easy refernce to and allow us to have a central singleton object that contains
/// all references to our HUD components
/// </summary>
public class InGameHUD : MonoBehaviour
{
    #region static variables
    private static InGameHUD instance;

    public static InGameHUD Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<InGameHUD>();
            }
            return instance;
        }
    }
    #endregion static variables


    [Header("Important HUD Components")]
    public DialogueManager DialogueManagerComponent;

    #region monobehaviour methods
    private void Awake()
    {
        instance = this;
    }
    #endregion monobehaviour methods

}
