using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBoxCollider2D : CustomCollider2D
{
    #region bounding variables
    public Vector2 UpLeftBounds { get { return Box2DBounds.UpLeft; } }
    public Vector2 UpRightBounds { get { return Box2DBounds.UpRight; } }
    public Vector2 DownLeftBounds { get { return Box2DBounds.DownLeft; } }
    public Vector2 DownRightBounds { get { return Box2DBounds.DownRight; } }
    #endregion bounding variables

    public CollisionFactory.Box2DBounds Box2DBounds;
    public Vector2 BoxColliderSize = Vector2.one;
    private CustomPhysics2D AssociatePhysicsComponent;

    #region monobehaviour methods
    protected override void Awake()
    {
        base.Awake();
        Box2DBounds = (CollisionFactory.Box2DBounds)CollisionFactory.GetNewBoundsInstance(CollisionFactory.ECollisionShape.Box);
        
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

        UnityEditor.Handles.color = DebugColliderColor;
        UnityEditor.Handles.DrawAAPolyLine(Box2DBounds.GetVerticies());

    }
#endif

    #endregion monobehaviour methods

    #region override methods
    public override void UpdateColliderBounds()
    {
        Vector2 AdjustedCeneterPoint = transform.position;
        AdjustedCeneterPoint += (ColliderOffset * transform.root.localScale);
        Vector2 BoxSize = BoxColliderSize * transform.root.localScale;

        if (IsCharacterCollider)
        {
            AdjustedCeneterPoint += (Vector2.up * BoxSize.y / 2);
        }

        Box2DBounds.UpdateColliderBounds(AdjustedCeneterPoint, BoxSize);
    }
    #endregion override methods
}
