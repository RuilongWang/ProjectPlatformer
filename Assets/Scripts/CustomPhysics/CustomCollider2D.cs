using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        PHYSICS,//
    }


    private CollisionFactory.Bounds AssociatedBounds;
    [Tooltip("The assigned collision type of our collision component. This will determine how we handle updates in our Collision manager for optimization")]
    public CollisionType AssignedCollisionType;
    [Tooltip("Mark this true if you are using a character. This will make it so that our collider is formed around their feet as a center point. This makes it easier for scaling while in game")]
    public bool IsCharacterCollider;
    
    [Tooltip("The offset of our collider bounds.")]
    public Vector2 ColliderOffset;
    #region monobehaivour methods
    protected virtual void Start()
    {

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
    #endregion virtual methods

}
