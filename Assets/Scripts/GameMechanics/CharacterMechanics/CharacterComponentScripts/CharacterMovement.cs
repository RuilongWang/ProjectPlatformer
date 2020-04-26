using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CustomPhysics2D))]
/// <summary>
/// This will function as a component to our Character. Allows our player to move around the world
/// </summary>
public class CharacterMovement : MonoBehaviour
{
    [Header("Grounded Values")]
    public float MaxWalkSpeed;
    public float MaxRunSpeed;

    [Header("Jump Values")]
    public float JumpHeight;
    public float TimeToReachJumpApex;

    /// <summary>
    /// Input variable
    /// </summary>
    private float HorizontalInput;
    private float VerticalInput;


    #region monobehaviour methods

    private void Awake()
    {
        
    }

    private void Update()
    {
        
    }
    #endregion monobehaviour methods

    #region input mehtods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="HorizontalInput"></param>
    public virtual void ApplyHorizontalInput(float HorizontalInput)
    {
        this.HorizontalInput = HorizontalInput;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="VerticalInput"></param>
    public virtual void ApplyVerticalInput(float VerticalInput)
    {
        this.VerticalInput = VerticalInput;
    }
    #endregion input methods

    #region ground movement methods

    #endregion ground movement methods

    #region in air methods

    #endregion in air methods
}
