﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Typically will only interact with the environment. Interactions with enemies or projectiles or other hitboxes should take place with a
/// hitbox trigger collider
/// </summary>
public class CustomPhysics2D : MonoBehaviour {
    #region const variables
    public const float GRAVITY_CONSTANT = 9.8f;
    #endregion const variables

    #region main variables

    public Vector2 Velocity = Vector2.zero;
    [Header("Gravity Values")]
    [Tooltip("When this is marked true, gravity will effect the object based on the gravity scale and gravity vector")]
    public bool useGravity = true;
    [Tooltip("If this is marked true, then the object will stop accelerating once it has reached the maximum velocity that it can travel")]
    public bool useTerminalVelocity = true;

    [SerializeField]
    [Tooltip("This will be marked true when the animator is overriding the velocity of this actor to force movement.")]
    public bool UseAnimatorVelocity;

    [SerializeField]
    [Tooltip("The direction that gravity will be acting on the object")]
    private Vector2 gravityVector = Vector2.down;

    [System.NonSerialized]
    /// <summary>
    /// This will be a count of the number of objects that we are touching in any given direction. If the value is negative, this implies that we are hitting something in the negative direction
    /// 
    /// For example isTouchingSide = 1, -1 means we are on the ground and touching a wall to our right
    /// </summary>
    public Vector2Int isTouchingSide = Vector2Int.zero;

    public List<CustomCollider2D> allCustomColliders { get; private set; }
    

    /// <summary>
    /// A boolean value that indicates whether or not our character is currently in the 
    /// </summary>
    public bool isInAir { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public Vector2 gravityRight { get { return new Vector2(-gravityVector.y, gravityVector.x); } }
    public Vector2 gravityLeft { get { return new Vector2(gravityVector.y, -gravityVector.x); } }
    public Vector2 gravityUp { get { return -gravityVector; } }
    public Vector2 gravityDown { get { return gravityVector; } }


    [Tooltip("The scale at which gravity can effect the object. Potentially can be used for varying the jump feel")]
    public float gravityScale = 1;
    [Tooltip("The maximum speed at which the ")]
    public float terminalVelocity = 10;
    #endregion main variables

    #region monobehavoiur methods
    private void Awake()
    {
        allCustomColliders = new List<CustomCollider2D>();
        Overseer.Instance.PhysicsManager.AddCustomPhysics(this);
    }

    private void OnDestroy()
    {
        if (Overseer.Instance && Overseer.Instance.PhysicsManager)
        {
            Overseer.Instance.PhysicsManager.RemoveCustomPhysics(this);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void UpdatePhysics()
    {
        UpdatePositionFromVelocity();
    }

    private void OnValidate()
    {
        gravityVector = gravityVector.normalized;
    }
    #endregion monobehaviour methods

    #region event methods
    /// <summary>
    /// This event will be called anytime our object lands on a ground tile after being considered airborne
    /// </summary>
    public delegate void PhysicsObjectGroundedEvent();
    public event PhysicsObjectGroundedEvent OnGroundedEvent;

    /// <summary>
    /// This event will be called anytime our object becomes airborne after being considered grounded
    /// </summary>
    public delegate void PhysicsObjectAirborneEvent();
    public event PhysicsObjectAirborneEvent OnAirborneEvent;
    #endregion event methods

    /// <summary>
    /// Updates the current velocity that is caused by gravitational force
    /// </summary>
    public void UpdateVelocityFromGravity()
    {
        if (!isInAir && Mathf.Abs(Velocity.y) > 0)
        {
            OnPhysicsObjectAirborne();
            this.isInAir = true;
        }

        if (!useGravity || UseAnimatorVelocity)
        {
            return;
        }

        if (useTerminalVelocity)
        {
            float dotGravity = Vector2.Dot(gravityVector, Velocity);
            Vector2 downComponent = dotGravity * gravityVector;
            Vector2 rightComponent = Vector2.Dot(gravityRight, Velocity) * gravityRight;

            if (downComponent.magnitude > terminalVelocity && dotGravity > 0)
            {
                Velocity = rightComponent + gravityDown * terminalVelocity;
            }
        }
        float gravityValueToApply = gravityScale * GRAVITY_CONSTANT * Overseer.DELTA_TIME;
        Velocity += gravityValueToApply * gravityVector;
    }

    /// <summary>
    /// 
    /// </summary>
    private void UpdatePositionFromVelocity()
    {
        
        Vector3 velocityVector3 = new Vector3(Velocity.x, Velocity.y, 0);
        
        this.transform.position += velocityVector3 * Overseer.DELTA_TIME;

    }

    /// <summary>
    /// Sets the direction that gravity will be applied if applicable
    /// </summary>
    /// <param name="gravityVector"></param>
    public void SetGravityVector(Vector2 gravityVector)
    {
        this.gravityVector = gravityVector.normalized;
    }

    /// <summary>
    /// Used to activate our character grounded trigger
    /// </summary>
    public void OnPhysicsObjectGrounded()
    {
        if (OnGroundedEvent == null) return;
        OnGroundedEvent();
    }

    /// <summary>
    /// Used to activate our character airborne trigger
    /// </summary>
    public void OnPhysicsObjectAirborne()
    {
        if (OnAirborneEvent == null) return;
        OnAirborneEvent();
    }
 
}
