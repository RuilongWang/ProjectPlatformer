using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CustomPhysics2D))]
public class MovementMechanics : MonoBehaviour {

    public const float WALK_THRESHOLD = .1f;
    public const float RUN_THRESHOLD = .65f;

    #region main variables
    [Header("Ground Speed Variables")]

    [Tooltip("Our goal speed when our character is running")]
    public float maxRunSpeed = 20f;
    [Tooltip("Our goal speed when our character is walking")]
    public float maxWalkSpeed = 9f;
    [Tooltip("The acceleration of the player's movement. Smoothes into its goal speeds")]
    public float acceleration = 35;


    [Space(3)]
    [Header("Jump Variables")]
    [Tooltip("The maximum speed that our character will while in the air")]
    public float maxInAirSpeed = 25;
    [Tooltip("The height of our jump at its max. Based on Unity units for distance")]
    public float heightOfJump = 1;
    [Tooltip("How long it takes before we reach the top of our jump height. Meausred in seconds")]
    public float timeToReachJumpApex = 1;
    [Tooltip("The acceleration that will be applied to our character while they are in the air.")]
    public float inAirAcceleration = 15;
    [Tooltip("The velocity that our character will launch with upward when performing a jump")]
    private float jumpVelocity;
    /// <summary>
    /// Indicates whethere or not our character can double jump in the air
    /// </summary>
    private bool doubleJumpAvailable = true;

    public bool canJump { get; private set; }
    #endregion main variables

    #region set at runtime
    public float verticalInput { get; set; }

    private float goalSpeed = 0;
    private CustomPhysics2D rigid;
    #endregion set at runtime

    #region monobehaviour methods
    private void Awake()
    {
        rigid = GetComponent<CustomPhysics2D>();
    }

    private void Start()
    {
        rigid.OnGroundedEvent += this.OnGroundedEvent;
    }

    private void OnValidate()
    {
        if (!rigid)
        {
            rigid = GetComponent<CustomPhysics2D>();
        }
        float gravity = (2 * heightOfJump) / Mathf.Pow(timeToReachJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToReachJumpApex;
        rigid.gravityScale = gravity / CustomPhysics2D.GRAVITY_CONSTANT;
    }

    private void Update()
    {
        UpdateVelocity();
    }

    private void OnDestroy()
    {
        rigid.OnGroundedEvent -= this.OnGroundedEvent;//Make sure to unsubscribe from the event avoid errors
    }
    #endregion monobehaviour methos

    #region set movement
    /// <summary>
    /// Sets the horizontal input that determines the speed that the character will be traveling
    /// </summary>
    /// <param name="horizontalInput"></param>
    public void SetHorizontalInput(float horizontalInput)
    {
        if (rigid.isInAir)
        {
            if (Mathf.Abs(horizontalInput) < WALK_THRESHOLD)
            {
                goalSpeed = rigid.velocity.x;

            }
            else
            {
                goalSpeed = Mathf.Sign(horizontalInput) * maxInAirSpeed;
            }
            return;
        }

        if (Mathf.Abs(horizontalInput) < WALK_THRESHOLD)
        {
            
            goalSpeed = 0;
            
        }
        else if (Mathf.Abs(horizontalInput) < RUN_THRESHOLD)
        {
            goalSpeed = maxWalkSpeed * Mathf.Sign(horizontalInput);
        }
        else
        {
            goalSpeed = maxRunSpeed * Mathf.Sign(horizontalInput);
        }
    }
    #endregion set movement
    /// <summary>
    /// This method will update the velocity to the goal velocity by the 
    /// </summary>
    private void UpdateVelocity()
    {
        float xVelocity = rigid.velocity.x;
        
        xVelocity = Mathf.MoveTowards(rigid.velocity.x, goalSpeed, Time.deltaTime * (rigid.isInAir ? inAirAcceleration : acceleration));
        
        
        rigid.velocity = new Vector2(xVelocity, rigid.velocity.y);
    }

    #region jump methods
    /// <summary>
    /// Call this method to perform a jump.
    /// Returns true if jump was successfully made. False if there 
    /// were conditions that were not met to perform a jump.
    /// </summary>
    /// <returns></returns>
    public bool Jump()
    {
        if (rigid.isInAir)
        {
            if (doubleJumpAvailable)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, jumpVelocity);
                doubleJumpAvailable = false;
                return true;
            }
            return false;
        }
        rigid.velocity = new Vector2(rigid.velocity.x, jumpVelocity);
        return true;
    }

    #endregion jump methods
    /// <summary>
    /// Use this method to run any code that needs to occur when we land
    /// </summary>
    public void OnGroundedEvent()
    {
        doubleJumpAvailable = true;
    }
}
