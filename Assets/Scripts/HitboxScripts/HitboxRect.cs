﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Our custom collider hitbox class. Hitboxes do not interact with any other type of collider except for other hitboxes and hurtboxes
/// Do not use this to interact with the environment or activate the triggers
/// </summary>
public class HitboxRect : Hitbox
{
    public Vector2 PositionOffset;
    public Vector2 BoxSize = Vector2.one;

    private CollisionFactory.Box2DBounds Box2DBounds;

    #region monobehaviour methods
    protected override void Awake()
    {
        base.Awake();
        Box2DBounds = (CollisionFactory.Box2DBounds)CollisionFactory.GetNewBoundsInstance(CollisionFactory.ECollisionShape.BOX);
        AssignHitboxBounds(Box2DBounds);
    }

    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR

        if (Box2DBounds == null)
        {
            Box2DBounds = (CollisionFactory.Box2DBounds)CollisionFactory.GetNewBoundsInstance(CollisionFactory.ECollisionShape.BOX);
        }
        if (!Application.isPlaying)
        {
            UpdateHitboxBounds();
        }
        Color ColorWithTransparency = DebugDrawColor;
        ColorWithTransparency.a = .2f;
        UnityEditor.Handles.DrawSolidRectangleWithOutline(Box2DBounds.GetVerticies(), ColorWithTransparency, DebugDrawColor);
        #endif

    }
    #endregion monobehaviour methods

    #region override methods
    /// <summary>
    /// Updates the bounds of our hitbox rect objects
    /// </summary>
    public override void UpdateHitboxBounds()
    {
        Vector2 TransformPositionVector2 = this.transform.position;
        //The updated bounds is based on the size, offset, and root transform local scale
        Vector2 ScaledOffset = (Vector2)this.transform.localScale * (Vector2)this.transform.root.localScale;
        Vector2 CenterOfHitbox = TransformPositionVector2 + PositionOffset * ScaledOffset;
        Vector2 SizeOfHitbox = this.BoxSize * ScaledOffset;
        Box2DBounds.SetColliderBoundsForBox2D(ref CenterOfHitbox, ref SizeOfHitbox);
    }
    #endregion override methods
}
