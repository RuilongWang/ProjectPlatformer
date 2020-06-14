using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CustomPhysics2D))]
/// <summary>
/// This will function as a component to our Character. Allows our player to move around the world
/// </summary>
public class CharacterMovement : MonoBehaviour
{
    #region const variables
    /// <summary>
    /// The Input thresholds for our walk and run movement while grounded
    /// 
    /// NOTE: Please remember to update these values in the character animator so that they appropriately line up with the values here
    /// </summary>
    private const float INPUT_WALK_THRESHOLD = .25f;
    private const float INPUT_RUN_THRESHOLD = .6f;
    #endregion const variables

    #region enum values
    /// <summary>
    /// Sets the current movement state of our character
    /// </summary>
    public enum MovementState : byte
    {
        STANDING_GROUNDED = 0x00, //Character is grounded in the neutral position
        CROUCHING_GROUNDED = 0x01, //Character is grounded in the crouching position
        IN_AIR = 0x02, //Character is currently in the air
    }

    /// <summary>
    /// Substate while our character is standed and grounded
    /// </summary>
    public enum GroundedStandingState : byte
    {
        IDLE = 0x00,
        WALK = 0x01,
        RUN = 0x02,
    }
    #endregion enum values

    #region member variables
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
    public float JumpHeight = 3.5f;
    [Tooltip("The time it will take for the character to reach the highest part of their jump")]
    public float TimeToReachJumpApex = 1;
    [Tooltip("The number of jumps that can be performed after the player is deemed in the air")]
    public int DoubleJumpCount = 1;
    [Tooltip("This is the scaled acceleration of our fall speed. This will be applied when you let go of the jump button or hold the down input")]
    public float FastFallScaled = 1.75f;


    /// <summary>
    /// This is the launch spaeed that we will use when calling the Jump method
    /// </summary>
    private float JumpVelocity = 5;
    /// <summary>
    /// 
    /// </summary>
    private float JumpingAcceleration = 1;

    /// <summary>
    /// Input variable that store the intended directions based on what our controllers pass in
    /// </summary>
    public Vector2 MovementInput { get; private set; }

    /// <summary>
    /// The previous vertical and horizontal input set by the controller
    /// </summary>
    private Vector2 PreviousMovementInput;

    /// <summary>
    /// The remaining number jumps that we can execute in the air 
    /// </summary>
    private int DoubleJumpsRemaining;

    /// <summary>
    /// The current movement state of our character
    /// </summary>
    public MovementState CurrentMovementState { get; private set; }

    /// <summary>
    /// This state will be updated while our character is in the grounded standing state
    /// </summary>
    public GroundedStandingState CurrentGroundedStandingState { get; private set; }

    [Header("Character Orientation")]
    [Tooltip("This will indicate that our character is facing in the 'Right' direction. This translates to our character's sprite renderer object have a positive localscale on the x-axis")]
    public bool IsCharacterFacingRight = true;

    /// <summary>
    /// A reference to the associated character component
    /// </summary>
    private GamePlayCharacter AssociatedCharacter;

    #region animator variables
    /// <summary>
    /// Overrides our velocity update with the desired velocity from our animation. There will still be acceleration applied, but goal velocity is strictly set from our animation
    /// </summary>
    public bool OverrideMovementWithAnimation;
    #endregion animator varialbes
    #endregion member variables

    #region component references
    /// <summary>
    /// Physics component that will updated with our character movement
    /// </summary>
    private CustomPhysics2D Rigid { get { return AssociatedCharacter.Rigid; } }
    #endregion component references

    #region monobehaviour methods
    private void Awake()
    {
        AssociatedCharacter = GetComponent<GamePlayCharacter>();
        DoubleJumpsRemaining = DoubleJumpCount;

    }

    private void Start()
    {
        AdjustGravityBasedOnJumpValues();
    }

    private void Update()
    {
        UpdateMovementBasedOnMovementState(CurrentMovementState);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (AssociatedCharacter == null)
        {
            AssociatedCharacter = GetComponent<GamePlayCharacter>();
        }
        TimeToReachJumpApex = Mathf.Max(0.05f, TimeToReachJumpApex);

        if (!Application.isPlaying)
            AdjustGravityBasedOnJumpValues();
    }
#endif
    #endregion monobehaviour methods

    #region private helper methods
    /// <summary>
    /// Updates our character's speed based on the movement type that they are currently assigned
    /// </summary>
    private void UpdateMovementBasedOnMovementState(MovementState MovementState)
    {
        switch (MovementState)
        {
            case MovementState.STANDING_GROUNDED:
                UpdateStandingGroundedMovement();
                if (Rigid.Velocity.y != 0)
                {
                    CurrentMovementState = MovementState.IN_AIR;
                    OnCharacterAirborne();
                }

                UpdateCharacterDirectionBasedOnInput();
                return;
            case MovementState.CROUCHING_GROUNDED:
                UpdateCrouchingGroundedMovement();
                return;
            case MovementState.IN_AIR:
                UpdateInAirMovement();
                if (Rigid.Velocity.y == 0)
                {
                    CurrentMovementState = MovementState.STANDING_GROUNDED;
                    OnCharacterLanded();
                }
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
        float AdjustedHorizontalMovement = (PreviousMovementInput.x + HorizontalInput) / 2;

        PreviousMovementInput = MovementInput;
        MovementInput = new Vector2(AdjustedHorizontalMovement, MovementInput.y);

    }

    /// <summary>
    /// Sets the vertical input for this character movement. This will be used to calculate the vertical movement of our character based on the movement
    /// type that they are currently assigned
    /// </summary>
    /// <param name="VerticalInput"></param>
    public virtual void ApplyVerticalInput(float VerticalInput)
    {
        float AdjustedVerticalMovement = (PreviousMovementInput.y + VerticalInput) / 2;

        PreviousMovementInput = MovementInput;
        MovementInput = new Vector2(MovementInput.x, AdjustedVerticalMovement);

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

        if (Mathf.Abs(MovementInput.x) > INPUT_RUN_THRESHOLD)
        {
            GoalSpeed = Mathf.Sign(MovementInput.x) * MaxRunSpeed;
            if (CurrentGroundedStandingState != GroundedStandingState.RUN) CurrentGroundedStandingState = GroundedStandingState.RUN;
        }
        else if (Mathf.Abs(MovementInput.x) > INPUT_WALK_THRESHOLD)
        {
            GoalSpeed = Mathf.Sign(MovementInput.x) * MaxWalkSpeed;
            if (CurrentGroundedStandingState != GroundedStandingState.WALK) CurrentGroundedStandingState = GroundedStandingState.WALK;
        }
        else
        {
            if (CurrentGroundedStandingState != GroundedStandingState.IDLE) CurrentGroundedStandingState = GroundedStandingState.IDLE;
        }
        //GoalSpeed = -1 * MaxRunSpeed;
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
        if (Mathf.Abs(MovementInput.x) > INPUT_WALK_THRESHOLD)
        {
            GoalSpeed = Mathf.Sign(MovementInput.x) * MaxCrouchSpeed;
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
        if (Mathf.Abs(MovementInput.x) > INPUT_WALK_THRESHOLD)
        {
            Vector2 CurrentVelocity = Rigid.Velocity;
            float GoalSpeed = Mathf.Sign(MovementInput.x) * MaxRunSpeed;

            CurrentVelocity.x = Mathf.MoveTowards(CurrentVelocity.x, GoalSpeed, GameOverseer.DELTA_TIME * AirAcceleration * Mathf.Abs(MovementInput.x));
            Rigid.Velocity = CurrentVelocity;
        }
    }
    #endregion in air methods

    #region jumping methods
    /// <summary>
    /// If valid, this will allow the player to Jump
    /// </summary>
    public void Jump()
    {
        if (CurrentMovementState != MovementState.IN_AIR)
        {
            Rigid.Velocity.y = JumpVelocity;
        }
        else if (DoubleJumpsRemaining > 0)
        {
            --DoubleJumpsRemaining;
            Rigid.Velocity.y = JumpVelocity;
        }
        else
        {
            return;
        }
        SetGravityScaleToFastFallScale(false);
    }

    /// <summary>
    /// Use this to notify that the player has released the jump. You can use this method to begin fast falling
    /// </summary>
    public void JumpReleased()
    {
        SetGravityScaleToFastFallScale(true);
    }

    private void SetGravityScaleToFastFallScale(bool ShouldSetGravityScaleToFastFallScale)
    {
        if (ShouldSetGravityScaleToFastFallScale)
        {
            Rigid.gravityScale = FastFallScaled * JumpingAcceleration;
        }
        else
        {
            Rigid.gravityScale = JumpingAcceleration;
        }
    }

    /// <summary>
    /// Use this method to reset properties for our character when they have landed. for example this is where we would
    /// want to reset the double jumps remaining
    /// </summary>
    private void OnCharacterLanded()
    {
        DoubleJumpsRemaining = DoubleJumpCount;
        CurrentMovementState = MovementState.STANDING_GROUNDED;
        SetGravityScaleToFastFallScale(false);
    }

    private void OnCharacterAirborne()
    {
        CurrentMovementState = MovementState.IN_AIR;
    }

    private void AdjustGravityBasedOnJumpValues()
    {
        if (!AssociatedCharacter || !Rigid)
        {
            Debug.LogWarning("Associated Character not set. Skipping Gravity Adjustment. This should not cause any issues if this is the first we created our character movement comoponent");
            return;
        }
        float gravity = (2 * JumpHeight) / Mathf.Pow(TimeToReachJumpApex, 2);
        JumpVelocity = Mathf.Abs(gravity * TimeToReachJumpApex);
        JumpingAcceleration = gravity / CustomPhysics2D.GRAVITY_CONSTANT;
        Rigid.gravityScale = JumpingAcceleration;

    }

    private IEnumerator AccelerateToZeroGravityVelocity(int FramesToReachZero)
    {
        float gravitySpeedAtStart = Rigid.Velocity.y;

        while (Rigid.Velocity.y < 0)
        {
            yield return null;
        }
    }
    #endregion jumping methods

    #region helper methods
    /// <summary>
    /// Checks to make sure that our direction bool is set to the correct direction based on the
    /// controller input
    /// </summary>
    private void UpdateCharacterDirectionBasedOnInput()
    {
        if (MovementInput.x < -INPUT_WALK_THRESHOLD && IsCharacterFacingRight)
            IsCharacterFacingRight = false;
        else if (MovementInput.x > INPUT_WALK_THRESHOLD && !IsCharacterFacingRight)
            IsCharacterFacingRight = true;
    }
    #endregion helper methods
}
