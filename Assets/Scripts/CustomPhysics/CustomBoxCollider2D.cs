﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SocialPlatforms;

public class CustomBoxCollider2D : CustomCollider2D
{
    #region bounding variables
    public Vector2 UpLeftBounds { get { return Box2DBounds.UpLeft; } }
    public Vector2 UpRightBounds { get { return Box2DBounds.UpRight; } }
    public Vector2 DownLeftBounds { get { return Box2DBounds.DownLeft; } }
    public Vector2 DownRightBounds { get { return Box2DBounds.DownRight; } }
    #endregion bounding variables

    public CollisionFactory.Box2DBounds Box2DBounds;
    public CollisionFactory.Box2DBounds PhysicsBoxBounds;
    public Vector2 BoxColliderSize = Vector2.one;
    [Tooltip("This is the buffer that we will give our physics colliders so that we do not run into instances where we collide with a wall in the opposite direction of our movement.")]
    public Vector2 BufferSizePhysicsCollision = Vector2.zero;

    #region monobehaviour methods
    protected override void Awake()
    {
        Box2DBounds = (CollisionFactory.Box2DBounds)CollisionFactory.GetNewBoundsInstance(CollisionFactory.ECollisionShape.BOX);
        if (AssignedCollisionType == CollisionType.PHYSICS)
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
            UpdateColliderBounds();
        }
    }

    private void OnDrawGizmos()
    {
        DrawBoxColliderBounds(Box2DBounds, DebugColliderColor);
        if (PhysicsBoxBounds != null)
            DrawBoxColliderBounds(PhysicsBoxBounds, Color.red);
    }
#endif

    private void DrawBoxColliderBounds(CollisionFactory.Box2DBounds BoxBounds, Color ColorToDraw)
    {
        UnityEditor.Handles.color = ColorToDraw;
        UnityEditor.Handles.DrawAAPolyLine(BoxBounds.GetVerticies());
    }

    #endregion monobehaviour methods

    #region override methods
    /// <summary>
    /// 
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

    public override void UpdatePhysicsColliderBounds()
    {

        Vector2 OffsetFromVelocity = (AssociatedPhysicsComponent.Velocity * GameOverseer.DELTA_TIME);
        Vector2 NewBoxSize = Box2DBounds.BoxSize + new Vector2(Mathf.Abs(OffsetFromVelocity.x), Mathf.Abs(OffsetFromVelocity.y));
        Vector2 NewBoxCenter = Box2DBounds.CenterPoint + OffsetFromVelocity / 2f;
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="OtherCollider"></param>
    protected override void HorizontallyPushOutCollider(CustomCollider2D OtherCollider)
    {
        Vector3 Offset = GetOffsetForNearestHorizontalPointOnBoundsForCollider(OtherCollider);
        OtherCollider.transform.position = OtherCollider.transform.position + Offset;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="OtherCollider"></param>
    protected override void VerticallyPushOutCollider(CustomCollider2D OtherCollider)
    {
        Vector3 Offset = GetOffsetForNearesVerticalPointOnBoundsForCollider(OtherCollider);
        OtherCollider.transform.position = OtherCollider.transform.position + Offset;
    }
    #endregion override methods
}
