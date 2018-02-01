using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CustomPhysics2D))]
public class MovementMechanics : MonoBehaviour {

    public const float WALK_THRESHOLD = .1f;
    public const float RUN_THRESHOLD = .65f;

    #region main variables
    [Tooltip("Our goal speed when our character is running")]
    public float maxRunSpeed = 20f;
    [Tooltip("Our goal speed when our character is walking")]
    public float maxWalkSpeed = 9f;

    [Tooltip("The acceleration of the player's movement. Smoothes into its goal speeds")]
    public float acceleration = 35;
    #endregion main variables

    #region set at runtime
    public float verticalInput { get; set; }

    private float goalSpeed = 0;
    private CustomPhysics2D rigid;
    #endregion set at runtime

    #region monobehaviour methods
    private void Start()
    {
        rigid = GetComponent<CustomPhysics2D>();
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
}
