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

    [Tooltip("The height of our jump at its max. Based on Unity units for distance")]
    public float heightOfJump = 1;
    [Tooltip("How long it takes before we reach the top of our jump height. Meausred in seconds")]
    public float timeToReachJumpApex = 1;
    [Tooltip("The velocity that our character will launch with upward when performing a jump")]
    private float jumpVelocity;

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
    #endregion monobehaviour methos

    #region set movement
    /// <summary>
    /// Sets the horizontal input that determines the speed that the character will be traveling
    /// </summary>
    /// <param name="horizontalInput"></param>
    public void SetHorizontalInput(float horizontalInput)
    {
        

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

    private void UpdateVelocity()
    {
        float xVelocity = Mathf.MoveTowards(rigid.velocity.x, goalSpeed, Time.deltaTime * acceleration);
        rigid.velocity = new Vector2(xVelocity, rigid.velocity.y);
    }

    #region jump methods
    public void Jump()
    {
        rigid.velocity = new Vector2(rigid.velocity.x, jumpVelocity);
    }

    #endregion jump methods
}
