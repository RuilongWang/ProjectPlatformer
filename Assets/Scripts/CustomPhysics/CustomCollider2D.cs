using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Events;

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
    /// 
    /// NOTE: This value cannot be changed after a collider is first created
    /// </summary>
    public enum ECollisionType
    {
        STATIC,//Never expected to move. Good for most environmental colliders
        MOVABLE,//Can move, but does not trace for collision. Will also not be affected by other colliders it passes through
        PHYSICS,//This collider contains a physics component and will collide with objects due to physics.
        TRIGGER,//This collider contains no physical properties and instead only acts to generate overlap events
    }

    [Tooltip("Generates overlapping events with other Other colliders that can also")]
    public bool CanGenerateOverlapEvents;

    /// <summary>
    /// Delegate that will trigger every time we receive a being overlap event
    /// </summary>
    [NonSerialized]
    public UnityAction<CustomCollider2D> AOnBeginOverlap;
    /// <summary>
    /// Delegate that will trigger every time we receive an exit overlap event
    /// </summary>
    [NonSerialized]
    public UnityAction<CustomCollider2D> UA_OnEndOverlap;

    /// <summary>
    /// The generic reference to our collider's bounds component
    /// </summary>
    private CollisionFactory.Bounds AssociatedBounds;

    protected CollisionFactory.Bounds PreviousBounds;

    /// <summary>
    /// The associated physics component of our collider. If this is a static or moveable object, you more than likely should not have a physics component attached to this object.
    /// </summary>
    public CustomPhysics2D AssociatedPhysicsComponent { get; protected set; }

    [Tooltip("The assigned collision type of our collision component. This will determine how we handle updates in our Collision Manager for optimization")]
    public ECollisionType AssignedCollisionType;
    [Tooltip("Mark this true if you are using a character. This will make it so that our collider is formed around their feet as a center point. This makes it easier for scaling while in game")]
    public bool IsCharacterCollider;
    
    [Tooltip("The offset of our collider bounds.")]
    public Vector2 ColliderOffset;

    [Tooltip("The collision's assigned physics layer. You can set what layers will collide with other layers using the Layer Collision Matrix in Unity")]
    public int CollisionLayer;

    private HashSet<CustomCollider2D> OverlappingColliderSet = new HashSet<CustomCollider2D>();
    #region monobehaivour methods
    protected virtual void Awake()
    {
        GameOverseer.Instance.PhysicsManager.AddCollider2DToPhysicsManager(this);
        AssociatedPhysicsComponent = GetComponent<CustomPhysics2D>();
        if (AssociatedPhysicsComponent && AssignedCollisionType != ECollisionType.PHYSICS)
        {
            Debug.LogWarning("Your collider contains a physics component, but is not set to CollisionType - 'Physics'. Are you sure this is correct?");
        }
        else if (!AssociatedPhysicsComponent && AssignedCollisionType == ECollisionType.PHYSICS)
        {
            Debug.LogWarning("You collider does not contain a Physics component, but is assigned to CollisionType - 'Physics'. This can not work");
        }
        PreviousBounds = CollisionFactory.GetNewBoundsInstance(AssociatedBounds.CollisionShape);
        UpdateColliderBounds();//We want to update the collider bounds on awake
        PreviousBounds.CopyBoundsFrom(AssociatedBounds);//Make an exact copy for the first iteration
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

        CollisionLayer = this.gameObject.layer;

        if (AssignedCollisionType == ECollisionType.TRIGGER) CanGenerateOverlapEvents = true;
#endif 
    }
    #endregion monobehaviour methods
    public CollisionFactory.Bounds GetAssociatedBounds() { return AssociatedBounds; }

    /// <summary>
    /// This method needs to be called to appropriately run collision methods for our collider
    /// </summary>
    /// <param name="BoundsToAssign"></param>
    protected void AssignBoundsToCollider(CollisionFactory.Bounds BoundsToAssign)
    {
        this.AssociatedBounds = BoundsToAssign;
    }

    protected Vector2 GetOffsetForNearestHorizontalPointOnBoundsForCollider(CustomCollider2D OtherCollider)
    {
        return this.AssociatedBounds.GetOffsetToClosestHorizontalPointOnBounds(OtherCollider.AssociatedBounds);
    }

    protected Vector2 GetOffsetForNearesVerticalPointOnBoundsForCollider(CustomCollider2D OtherCollider)
    {
        return this.AssociatedBounds.GetOffsetToClosestVerticalPointOnBounds(OtherCollider.AssociatedBounds);
    }


    #region overlap methods

    /// <summary>
    /// This method will be called every time we generate an overlap event
    /// </summary>
    /// <param name="Collider2D"></param>
    public void OnBeginOverlap(CustomCollider2D Collider2D)
    {
        if (AOnBeginOverlap != null) AOnBeginOverlap.Invoke(Collider2D);
    }

    /// <summary>
    /// This method will be called every time we generate an end overlap event
    /// </summary>
    /// <param name="Collider2D"></param>
    public void OnEndOverlap(CustomCollider2D Collider2D)
    {
        if (UA_OnEndOverlap != null) UA_OnEndOverlap.Invoke(Collider2D);
    }
    #endregion overlap methods

    #region virtual methods
    /// <summary>
    /// This should be called every frame to appropiately update the collision bounds based on position, scale, etc.
    /// </summary>
    public abstract void UpdateColliderBounds();

    public abstract void UpdatePhysicsColliderBounds();

    /// <summary>
    /// Is our Collider overlapping the collider that is passed in
    /// </summary>
    /// <param name="OtherCollider"></param>
    /// <returns></returns>
    public virtual bool IsOverlappingCollider(CustomCollider2D OtherCollider)
    {
        return this.AssociatedBounds.IsOverlappingBounds(OtherCollider.AssociatedBounds);
    }

    public abstract bool IsPhysicsColliderOverlapping(CustomCollider2D OtherCollider);

    public Vector2 PushOutCollider(CustomCollider2D OtherCollider, out bool ShouldPushOutVertically, out bool ShouldPushOutHorizontally, bool UseBufferForOverlap = false)
    {
        Vector2 OffsetToBePushedOut = Vector2.zero;
        PreviousBounds.ShouldPushOutBounds(OtherCollider.AssociatedBounds, out ShouldPushOutVertically, out ShouldPushOutHorizontally, UseBufferForOverlap);
        if (ShouldPushOutVertically)
        {
            OffsetToBePushedOut += AssociatedBounds.GetOffsetToClosestVerticalPointOnBounds(OtherCollider.AssociatedBounds);
        }
        if (ShouldPushOutHorizontally)
        {
            OffsetToBePushedOut += AssociatedBounds.GetOffsetToClosestHorizontalPointOnBounds(OtherCollider.AssociatedBounds);
        }

        return OffsetToBePushedOut;
    }
    #endregion virtual methods

}
