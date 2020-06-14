using UnityEngine;

public class CustomBoxCollider2D : CustomCollider2D
{
    protected CollisionFactory.Box2DBounds Box2DBounds;
    protected CollisionFactory.Box2DBounds PhysicsBoxBounds;
    [Tooltip("The size of our box collider. Keep in mind that the size of the box collider will change with the local scale of transform that we are attached to")]
    public Vector2 BoxColliderSize = Vector2.one;
    [Tooltip("This is the buffer that we will give our physics colliders so that we do not run into instances where we collide with a wall in the opposite direction of our movement.")]
    public Vector2 BufferSizePhysicsCollision = Vector2.zero;

    #region monobehaviour methods
    protected override void Awake()
    {
        Box2DBounds = (CollisionFactory.Box2DBounds)CollisionFactory.GetNewBoundsInstance(CollisionFactory.ECollisionShape.BOX);
        if (AssignedCollisionType == ECollisionType.PHYSICS)
        {
            Box2DBounds.BufferBounds = BufferSizePhysicsCollision;
            PhysicsBoxBounds = (CollisionFactory.Box2DBounds)CollisionFactory.GetNewBoundsInstance(CollisionFactory.ECollisionShape.BOX);
        }
        AssignBoundsToCollider(Box2DBounds);
        base.Awake();
        
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying)
        {
            if (Box2DBounds == null)
            {
                Box2DBounds = new CollisionFactory.Box2DBounds();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            if (Box2DBounds == null) Box2DBounds = (CollisionFactory.Box2DBounds)CollisionFactory.GetNewBoundsInstance(CollisionFactory.ECollisionShape.BOX);
            UpdateColliderBounds();
        }

        DrawBoxColliderBounds(Box2DBounds, DebugColliderColor);
        if (PhysicsBoxBounds != null)
            DrawBoxColliderBounds(PhysicsBoxBounds, Color.red);

        
    }
#endif

    #endregion monobehaviour methods

    #region override methods
    /// <summary>
    /// Update the bounds of our box collider based on its new transform position
    /// </summary>
    public override void UpdateColliderBounds()
    {
        Vector2 AdjustedCeneterPoint = transform.position;
        AdjustedCeneterPoint += (ColliderOffset * transform.localScale);
        Vector2 BoxSize = BoxColliderSize * transform.localScale;

        if (IsCharacterCollider)//If this is a character, we will move the base of the collider to the character's feet
        {
            AdjustedCeneterPoint += (Vector2.up * BoxSize.y / 2);
        }

        Box2DBounds.SetColliderBoundsForBox2D(ref AdjustedCeneterPoint, ref BoxSize);
    }

    /// <summary>
    /// Updates the bounds of the physics collider based on the updated bounds position and the velocity of the physics object. This will only ever
    /// be called for colliders of type physics
    /// </summary>
    public override void UpdatePhysicsColliderBounds()
    {
        Vector2 OffsetFromVelocity = (AssociatedPhysicsComponent.Velocity * GameOverseer.DELTA_TIME);
        Vector2 NewBoxSize = Box2DBounds.BoxSize + new Vector2(Mathf.Abs(OffsetFromVelocity.x), Mathf.Abs(OffsetFromVelocity.y));
        Vector2 NewBoxCenter = Box2DBounds.CenterPoint + OffsetFromVelocity / 2f;
        if (AssociatedPhysicsComponent.Velocity.y != 0)
        {
            NewBoxCenter.y += Mathf.Sign(AssociatedPhysicsComponent.Velocity.y) * BufferSizePhysicsCollision.y;
            NewBoxSize.y -= BufferSizePhysicsCollision.y / 2;
        }
        if (AssociatedPhysicsComponent.Velocity.x != 0)
        {
            NewBoxCenter.x += Mathf.Sign(AssociatedPhysicsComponent.Velocity.x) * BufferSizePhysicsCollision.x;
            NewBoxSize.x -= BufferSizePhysicsCollision.x / 2;
        }
        PhysicsBoxBounds.SetColliderBoundsForBox2D(ref NewBoxCenter, ref NewBoxSize);
    }

    /// <summary>
    /// Override method to check if we are overlapping with the collider that is passed in
    /// </summary>
    /// <param name="OtherCollider"></param>
    /// <returns></returns>
    public override bool IsOverlappingCollider(CustomCollider2D OtherCollider)
    {
        return base.IsOverlappingCollider(OtherCollider);
    }

    public override bool IsPhysicsColliderOverlapping(CustomCollider2D OtherCollider)
    {
        return this.PhysicsBoxBounds.IsOverlappingBounds(OtherCollider.GetAssociatedBounds());
    }


    #endregion override methods

    #region debugging
    /// <summary>
    /// 
    /// </summary>
    /// <param name="BoxBounds"></param>
    /// <param name="ColorToDraw"></param>
    private void DrawBoxColliderBounds(CollisionFactory.Box2DBounds BoxBounds, Color ColorToDraw)
    {
        UnityEditor.Handles.color = ColorToDraw;
        UnityEditor.Handles.DrawAAPolyLine(BoxBounds.GetVerticies());
    }
    #endregion debugging
}
