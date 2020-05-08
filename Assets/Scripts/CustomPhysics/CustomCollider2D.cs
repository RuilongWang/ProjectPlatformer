using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

/// <summary>
/// Base class of our custom collider. This will check to see if there are any points where our collider intersects
/// with other colliders.
/// </summary>
public abstract class CustomCollider2D : MonoBehaviour {

    #region debugging
    protected readonly Color DebugColliderColor = Color.green;
    #endregion debugging


    /// <summary>
    /// The assigned type of collision this collision component is using
    /// </summary>
    public enum CollisionType
    {
        STATIC,//Never expected to move. Good for most environmental colliders
        MOVABLE,//Can move, but does not trace for collision.
        PHYSICS,//This collider contains a physics component and will collide with objects due to physics.
    }

    /// <summary>
    /// The generic reference to our collider's bounds component
    /// </summary>
    private CollisionFactory.Bounds AssociatedBounds;

    /// <summary>
    /// The associated physics component of our collider. If this is a static or moveable object, you more than likely should not have a physics component attached to this object.
    /// </summary>
    private CustomPhysics2D AssociatePhysicsComponent;

    [Tooltip("The assigned collision type of our collision component. This will determine how we handle updates in our Collision manager for optimization")]
    public CollisionType AssignedCollisionType;
    [Tooltip("Mark this true if you are using a character. This will make it so that our collider is formed around their feet as a center point. This makes it easier for scaling while in game")]
    public bool IsCharacterCollider;
    
    [Tooltip("The offset of our collider bounds.")]
    public Vector2 ColliderOffset;
    #region monobehaivour methods
    protected virtual void Awake()
    {
        GameOverseer.Instance.PhysicsManager.AddCollider2DToPhysicsManager(this);
        AssociatePhysicsComponent = GetComponent<CustomPhysics2D>();
        if (AssociatePhysicsComponent && AssignedCollisionType != CollisionType.PHYSICS)
        {
            Debug.LogWarning("Your collider contains a physics component, but is not set to CollisionType - 'Physics'. Are you sure this is correct?");
        }
        else if (!AssociatePhysicsComponent && AssignedCollisionType == CollisionType.PHYSICS)
        {
            Debug.LogWarning("You collider does not contain a Physics component, but is assigned to CollisionType - 'Physics'. This can not work");
        }
        UpdateColliderBounds();//We want to update the collider bounds on awake
    }

    protected virtual void Start()
    {

    }

    private void OnDestroy()
    {
        if (GameOverseer.Instance && GameOverseer.Instance.PhysicsManager)
        {
            GameOverseer.Instance.PhysicsManager.RemoveCollider2DFromPhysicsManager(this);
        }
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UpdateColliderBounds();
        }
#endif 
    }
    #endregion monobehaviour methods

    /// <summary>
    /// This method needs to be called to appropriately run collision methods for our collider
    /// </summary>
    /// <param name="BoundsToAssign"></param>
    protected void AssignBoundsToCollider(CollisionFactory.Bounds BoundsToAssign)
    {
        this.AssociatedBounds = BoundsToAssign;
    }


    #region virtual methods
    /// <summary>
    /// This should be called every frame to appropiately update the collision bounds based on position, scale, etc.
    /// </summary>
    public abstract void UpdateColliderBounds();

    /// <summary>
    /// Is our Collider overlapping the collider that is passed in
    /// </summary>
    /// <param name="OtherCollider"></param>
    /// <returns></returns>
    public virtual bool IsOverlappingCollider(CustomCollider2D OtherCollider)
    {
        return this.AssociatedBounds.IsOverlappingBounds(OtherCollider.AssociatedBounds);
    }
    #endregion virtual methods

}
