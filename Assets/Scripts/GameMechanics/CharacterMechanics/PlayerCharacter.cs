using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
/// <summary>
/// A child of the character class that will perform and hold values important to our Playable character
/// </summary>
public class PlayerCharacter : Character
{
    public PlayerController AssociatedPlayerController { get; private set; }

    #region monobehavoiur methods
    private void Awake()
    {
        AssociatedPlayerController = GetComponent<PlayerController>();
    }
    #endregion monobehaviour methods

    /// <summary>
    /// This method is where you should bind our player controller functions. Assign buttons and axes to their appropriate functions
    /// </summary>
    protected virtual void SetupPlayerController()
    {

    }
}
