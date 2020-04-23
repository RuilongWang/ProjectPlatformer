using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CustomPhysics2D))]
/// <summary>
/// This script is in charge of handling all logic that is related to character movement.
/// This script should be used for any Character that moves
/// </summary>
public class CharacterMovement : MonoBehaviour
{
    [Header("Grounded Values")]
    public float MaxWalkSpeed;
    public float MaxRunSpeed;

    [Header("Jump Values")]
    public float JumpHeight;
    public float TimeToReachJumpApex;

}
