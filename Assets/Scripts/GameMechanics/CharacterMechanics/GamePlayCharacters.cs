using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Extension of Character. These characters are meant to be characters that will have gameplay features. These characters are
/// expected to use movement and are typically expected to be controlled in some way. Either by our player or by AI controller
/// </summary>
public class GamePlayCharacters : Character
{
    /// <summary>
    /// 
    /// </summary>
    public CharacterMovement CharacterMovement { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public CustomPhysics2D Rigid { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public CharacterController AssignedCharacterController;

    protected override void Awake()
    {
        base.Awake();
        CharacterMovement = GetComponent<CharacterMovement>();
        Rigid = GetComponent<CustomPhysics2D>();
        AssignedCharacterController = GetComponent<CharacterController>();
    }
}
