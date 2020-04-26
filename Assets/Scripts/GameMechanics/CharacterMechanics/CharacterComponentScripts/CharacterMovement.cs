using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CustomPhysics2D))]
/// <summary>
/// This will function as a component to our Character. Allows our player to move around the world
/// </summary>
public class CharacterMovement : MonoBehaviour
{

    #region const variables
    private const float INPUT_WALK_THRESHOLD = .15f;
    private const float INPUT_RUN_THRESHOLD = .65f;
    #endregion const variables

    #region enum values
    /// <summary>
    /// Sets the current movement state of our character
    /// </summary>
    public enum MovementState
    {
        STANDING_GROUNDED,
        CROUCHING_GROUNDED,
        IN_AIR,
    }
    #endregion enum values
    [Header("Grounded Movement")]
    [Tooltip("The acceleration of our character toward their goal speed")]
    public float GroundAcceleration;
    [Tooltip("The maximum speed that our character can move while walking")]
    public float MaxWalkSpeed = 3;
    [Tooltip("The maximum speed that our character can move while running")]
    public float MaxRunSpeed = 6.5f;

    [Header("Crouching Movement")]
    public float MaxCrouchSpeed = 2.5f;

    [Header("In Air Movement")]
    [Tooltip("This will determine the amount of control the player has in the air")]
    public float AirAcceleration;

    [Header("Jump Values")]
    [Tooltip("The height that our character will reach at the highest point of their jump")]
    public float JumpHeight;
    [Tooltip("The time it will take for the character to reach the highest part of their jump")]
    public float TimeToReachJumpApex;
    [Tooltip("The number of jumps that can be performed after the player is deemed in the air")]
    public int DoubleJumpCount = 1;
    private float JumpVelocity = 5;

    /// <summary>
    /// Input variable that store the intended directions based on what our controllers pass in
    /// </summary>
    private float HorizontalInput;
    private float VerticalInput;
    private int DoubleJumpsRemaining;

    /// <summary>
    /// The current movement state of our character
    /// </summary>
    public MovementState CurrentMovementState;

    #region component references
    /// <summary>
    /// Physics component that will updated with our character movement
    /// </summary>
    private CustomPhysics2D Rigid;
    #endregion component references


    #region monobehaviour methods

    private void Awake()
    {
        
    }

    private void Update()
    {
        UpdateMovementBasedOnMovementState(CurrentMovementState);
    }

    private void OnDestroy()
    {
        
    }
    #endregion monobehaviour methods

    #region private helper methods
    /// <summary>
    /// Updates our character's speed based on the movement type that they are currently assigned
    /// </summary>
    private void UpdateMovementBasedOnMovementState(MovementState MovementState)
    {
        switch(MovementState)
        {
            case MovementState.STANDING_GROUNDED:
                UpdateStandingGroundedMovement();
                return;
            case MovementState.CROUCHING_GROUNDED:
                UpdateCrouchingGroundedMovement();
                return;
            case MovementState.IN_AIR:
                UpdateInAirMovement();
                return;
        }
    }
    #endregion private helper methods

    #region input mehtods
    /// <summary>
    /// Set the horizontal input for this character movement. This will be used to calculate the horizontal movement of our character based on the movement
    /// type that they are currently assigned
    /// </summary>
    /// <param name="HorizontalInput"></param>
    public virtual void ApplyHorizontalInput(float HorizontalInput)
    {
        this.HorizontalInput = HorizontalInput;
    }

    /// <summary>
    /// Sets the vertical input for this character movement. This will be used to calculate the vertical movement of our character based on the movement
    /// type that they are currently assigned
    /// </summary>
    /// <param name="VerticalInput"></param>
    public virtual void ApplyVerticalInput(float VerticalInput)
    {
        this.VerticalInput = VerticalInput;
    }
    #endregion input methods

    #region ground movement methods
    /// <summary>
    /// Updates the speed of the character when the character is grounded
    /// </summary>
    private void UpdateStandingGroundedMovement()
    {
        Vector2 CurrentVelocity = Rigid.Velocity;
        float GoalSpeed = 0;
        if (Mathf.Abs(HorizontalInput) > INPUT_RUN_THRESHOLD)
        {
            GoalSpeed = Mathf.Sign(HorizontalInput) * MaxRunSpeed;
        }
        else if (Mathf.Abs(HorizontalInput) > INPUT_WALK_THRESHOLD)
        {
            GoalSpeed = Mathf.Sign(HorizontalInput) * MaxWalkSpeed;
        }

        CurrentVelocity.x = Mathf.MoveTowards(CurrentVelocity.x, GoalSpeed, GameOverseer.DELTA_TIME * GroundAcceleration);
        Rigid.Velocity = CurrentVelocity;
        
    }

    /// <summary>
    /// Updates the spedd of our character when they are assigned to the Crouching_Grounded State
    /// </summary>
    private void UpdateCrouchingGroundedMovement()
    {
        Vector2 CurrentVelocity = Rigid.Velocity;
        float GoalSpeed = 0;
        if (Mathf.Abs(HorizontalInput) > INPUT_WALK_THRESHOLD)
        {
            GoalSpeed = Mathf.Sign(HorizontalInput) * MaxCrouchSpeed;
        }

        CurrentVelocity.x = Mathf.MoveTowards(CurrentVelocity.x, GoalSpeed, GameOverseer.DELTA_TIME * GroundAcceleration);
    }

    #endregion ground movement methods

    #region in air methods
    /// <summary>
    /// Updates our character's velocity based on inputs when our player is in the IN_AIR state 
    /// </summary>
    private void UpdateInAirMovement()
    {
        if (Mathf.Abs(HorizontalInput) > INPUT_WALK_THRESHOLD)
        {
            Vector2 CurrentVelocity = Rigid.Velocity;
            float GoalSpeed = Mathf.Sign(HorizontalInput) * MaxRunSpeed;

            CurrentVelocity.x = Mathf.MoveTowards(CurrentVelocity.x, GoalSpeed, GameOverseer.DELTA_TIME * AirAcceleration * Mathf.Abs(HorizontalInput));
        }
    }
    #endregion in air methods

    #region jumping methods
    /// <summary>
    /// If valid, this will allow the player to Jump
    /// </summary>
    public void Jump()
    {
        if (!Rigid.isInAir)
        {
            Rigid.Velocity.y = JumpVelocity;
        }
        else if (DoubleJumpsRemaining > 0)
        {
            --DoubleJumpsRemaining;
            Rigid.Velocity.y = JumpVelocity;
        }
    }

    /// <summary>
    /// Use this to notify that the player has released the jump. You can use this method to begin fast falling
    /// </summary>
    public void JumpReleased()
    {

    }

    /// <summary>
    /// Use this method to reset properties for our character when they have landed. for example this is where we would
    /// want to reset the double jumps remaining
    /// </summary>
    private void OnCharacterLanded()
    {
        DoubleJumpsRemaining = DoubleJumpCount;
    }
    #endregion jumping methods
}
