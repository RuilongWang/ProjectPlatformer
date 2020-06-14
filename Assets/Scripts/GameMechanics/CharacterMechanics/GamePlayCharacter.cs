using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterAttack))]
[RequireComponent(typeof(CustomPhysics2D))]
[RequireComponent(typeof(CharacterController))]


/// <summary>
/// Extension of Character. These characters are meant to be characters that will have gameplay features. These characters are
/// expected to use movement and are typically expected to be controlled in some way. Either by our player or by AI controller
/// </summary>
public class GamePlayCharacter : Character
{
    /// <summary>
    /// 
    /// </summary>
    public CharacterMovement CharacterMovementComponent { get; private set; }

    public CharacterAttack CharacterAttackComponent { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public CustomPhysics2D Rigid { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public CharacterController AssignedCharacterController { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        CharacterMovementComponent = GetComponent<CharacterMovement>();
        Rigid = GetComponent<CustomPhysics2D>();
        AssignedCharacterController = GetComponent<CharacterController>();
        CharacterAttackComponent = GetComponent<CharacterAttack>();
    }

    protected override void OnValidate()
    {
        base.OnValidate();

        if (Rigid == null) Rigid = GetComponent<CustomPhysics2D>();
        if (CharacterAttackComponent == null) CharacterAttackComponent = GetComponent<CharacterAttack>();
        if (CharacterMovementComponent == null) CharacterMovementComponent = GetComponent<CharacterMovement>();
    }
}
