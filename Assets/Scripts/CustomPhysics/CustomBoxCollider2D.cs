using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBoxCollider2D : MonoBehaviour
{
    #region bounding variables
    private CollisionFactory.Box2DBounds boxColliderBounds;

    public Vector2 UpLeftBounds { get { return boxColliderBounds.UpLeft; } }
    public Vector2 UpRightBounds { get { return boxColliderBounds.UpRight; } }
    public Vector2 DownLeftBounds { get { return boxColliderBounds.DownLeft; } }
    public Vector2 DownRightBounds { get { return boxColliderBounds.DownRight; } }
    #endregion bounding variables
}
