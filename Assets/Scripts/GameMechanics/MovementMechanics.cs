using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CustomPhysics2D))]
public class MovementMechanics : MonoBehaviour {
    #region const variables
    /// <summary>
    /// The value of the horizontal axis on the joystick before our character can walk
    /// </summary>
    public const float WALK_THRESHOLD = .1f;

    /// <summary>
    /// The value of the horizontal axis on the joystick before our character can run
    /// </summary>
    public const float RUN_THRESHOLD = .65f;
    #endregion const variables

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
    [Tooltip("The scale that we will multiply the gravity force when our character is fast falling")]
    public float fastFallScale = 1.7f;
    //Indicates whether or not our character is currently fast falling
    public bool isFastFalling { get; set; }
    [Tooltip("The velocity that our character will launch with upward when performing a jump")]
    private float jumpVelocity;

    [Space(3)]
    [Header("Dashing Variables")]
    [Tooltip("The amount of time in seconds that our character will be dashing")]
    [Range(.1f, 2f)]
    public float dashTime = 1f;
    [Tooltip("The speed of our character's dash movement")]
    public float dashSpeed = 8f;
    [Tooltip("The cool down time that will need to take place before you can use the dash mechanic again")]
    public float dashCoolDownTime = .5f;
    private bool dashAvailable = true;
    public bool isDashing { get; private set; }


    [Space(3)]
    [Header("Orientation Variables")]
    public bool isFacingRight;

    /// <summary>
    /// Indicates whether or not our character can double jump in the air
    /// </summary>
    private bool doubleJumpAvailable = true;

    public bool canJump { get; private set; }
    #endregion main variables

    #region set at runtime
    public float verticalInput { get; set; }

    private float goalSpeed = 0;
    private CustomPhysics2D rigid;

    private Animator anim;
    private float defaultGravityScale = 1;
    #endregion set at runtime

    #region monobehaviour methods
    private void Awake()
    {
        rigid = GetComponent<CustomPhysics2D>();
        defaultGravityScale = rigid.gravityScale;
    }

    private void Start()
    {
        rigid.OnGroundedEvent += this.OnGroundedEvent;
        anim = GetComponent<Animator>();
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

        FlipSpriteToFaceDirection();
    }

    private void Update()
    {
        UpdateGravityScaleWhenInAir();
        if (!isDashing)
        {
            UpdateVelocity();
        }
    }

    private void OnDestroy()
    {
        rigid.OnGroundedEvent -= this.OnGroundedEvent;//Make sure to unsubscribe from the event avoid errors and memory leaks
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

        anim.SetFloat("Speed", Mathf.Abs(horizontalInput));

        if (Mathf.Abs(horizontalInput) < WALK_THRESHOLD)
        {
            
            goalSpeed = 0;
            return;
            
        }
        else if (Mathf.Abs(horizontalInput) < RUN_THRESHOLD)
        {
            goalSpeed = maxWalkSpeed * Mathf.Sign(horizontalInput);
        }
        else
        {
            goalSpeed = maxRunSpeed * Mathf.Sign(horizontalInput);
        }

        if (goalSpeed > 0 && !isFacingRight)
        {
            isFacingRight = true;
            FlipSpriteToFaceDirection();
        }
        else if (goalSpeed < 0 && isFacingRight)
        {
            isFacingRight = false;
            FlipSpriteToFaceDirection();
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
    /// Updates the scale of gravity in our rigid variable based on whether or not we are fast
    /// falling
    /// </summary>
    private void UpdateGravityScaleWhenInAir()
    {
        if (!rigid.isInAir)
        {
            return;//Don't worry about this method if we are not currently in the air
        }

        rigid.gravityScale = defaultGravityScale * (isFastFalling ? fastFallScale : 1);
    }

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

    /// <summary>
    /// Use this method to run any code that needs to occur when we land
    /// </summary>
    public void OnGroundedEvent()
    {
        doubleJumpAvailable = true;
    }
    #endregion jump methods

    #region orientation methods
    /// <summary>
    /// Sets the scale of the sprite so that it is facing the direction that the player is having them
    /// move
    /// </summary>
    private void FlipSpriteToFaceDirection()
    {
        float adjustedXScale = Mathf.Round(Mathf.Abs(this.transform.localScale.x) * (isFacingRight ? 1 : -1) * 100) / 100;
        this.transform.localScale = new Vector3(adjustedXScale, this.transform.localScale.y, this.transform.localScale.z);
    }
    #endregion orientation methods

    #region dash methods
    public void Dash(float horizontalInput, float verticalInput)
    {
        isDashing = true;
        StartCoroutine(DashCoroutine(horizontalInput, verticalInput));
    }

    /// <summary>
    /// This coroutine will handle the speeds at which we will be moving in during the duration of our dash
    /// </summary>
    /// <returns></returns>
    public IEnumerator DashCoroutine(float horizontalInput, float verticalInput)
    {
        float timeToDecelerate = .1f;
        float timeAtFullSpeed = dashTime - timeToDecelerate;
        Vector2 directionOfDash = new Vector2(Mathf.RoundToInt(horizontalInput), Mathf.RoundToInt(verticalInput));
        if (directionOfDash == Vector2.zero)
        {
            directionOfDash = new Vector2(Mathf.Sign(transform.localScale.x), 0);//Just in case we are moving at 
        }
        directionOfDash = directionOfDash.normalized;
        rigid.useGravity = false;
        rigid.velocity = directionOfDash * dashSpeed;
        while (timeAtFullSpeed > 0)
        {
            if (!isDashing)
            {
                StartCoroutine(DashCoolDown(dashCoolDownTime));
                isDashing = false;
                rigid.useGravity = true;
                yield break;
            }
            timeAtFullSpeed -= Time.deltaTime;
            yield return null;
        }
        rigid.useGravity = true;
        isDashing = false;
        
    }

    public IEnumerator DashCoolDown(float timeBeforeCanDash)
    {
        yield return new WaitForSeconds(timeBeforeCanDash);
        dashAvailable = true;
    }

    public void CancelDash()
    {
        isDashing = false;
    }
    #endregion dash methods




}
