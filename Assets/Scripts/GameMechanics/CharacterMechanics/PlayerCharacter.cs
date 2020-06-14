using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A child of the character class that will perform and hold values important to our Playable character
/// </summary>
public class PlayerCharacter : GamePlayCharacter
{

    #region monobehavoiur methods
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnValidate()
    {
        base.OnValidate();
        if (AssignedCharacterController == null) AssignedCharacterController = GetComponent<PlayerController>();
    }
    #endregion monobehaviour methods
}
