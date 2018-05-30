using System.Collections;
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
    public Vector2 velocity { get; set; }
    [Header("Gravity Values")]
    [Tooltip("When this is marked true, gravity will effect the object based on the gravity scale and gravity vector")]
    public bool useGravity = true;
    [Tooltip("If this is marked true, then the object will stop accelerating once it has reached the maximum velocity that it can travel")]
    public bool useTerminalVelocity = true;
    [SerializeField]
    [Tooltip("The direction that gravity will be acting on the object")]
    private Vector2 gravityVector = Vector2.down;

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
    }

    private void Update()
    {
        if (useGravity) UpdateVelocityFromGravity();


        foreach (CustomCollider2D customCollider in allCustomColliders)
        {
            customCollider.UpdateCollisionPhysics();
        }
        UpdatePositionFromVelocity();
    }

    private void OnValidate()
    {
        gravityVector = gravityVector.normalized;
    }
    #endregion monobehaviour methods


    private void UpdateVelocityFromGravity()
    {
        if (useTerminalVelocity)
        {
            //Vector2 currentVelocityDown = new Vector2(velocity.x )
            float dotGravity = Vector2.Dot(gravityVector, velocity);
            Vector2 downComponent = dotGravity * gravityVector;
            Vector2 rightComponent = Vector2.Dot(gravityRight, velocity) * gravityRight;

            if (downComponent.magnitude > terminalVelocity && dotGravity > 0)
            {
                velocity = rightComponent + gravityDown * terminalVelocity;
            }
        }
        float gravityValueToApply = gravityScale * GRAVITY_CONSTANT * Time.deltaTime;
        velocity += gravityValueToApply * gravityVector;

    }

    private void UpdatePositionFromVelocity()
    {
        Vector3 velocityVector3 = new Vector3(velocity.x, velocity.y, 0);
        
        this.transform.position += velocityVector3 * Time.deltaTime;
    }

    /// <summary>
    /// Sets the direction that gravity will be applied if applicable
    /// </summary>
    /// <param name="gravityVector"></param>
    public void SetGravityVector(Vector2 gravityVector)
    {
        this.gravityVector = gravityVector.normalized;
    }
}
