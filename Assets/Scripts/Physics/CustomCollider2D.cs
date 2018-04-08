using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is specifically used do detect collisions of kinematic objects in their environemnt. It will only 
/// ensure that an object does not pass through a InteractableTIle object
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class CustomCollider2D : MonoBehaviour {
    public Collider2D associatedCollider;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;


    private CustomPhysics2D rigid;
    private ColliderBounds currentColliderBounds;


    #region monobehaviour methods
    private void Start()
    {
        rigid = GetComponent<CustomPhysics2D>();
        associatedCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        UpdateColliderBounds();
    }

    private void OnValidate()
    {
        if (horizontalRayCount < 2)
        {
            horizontalRayCount = 2;
        }
        if (verticalRayCount < 2)
        {
            verticalRayCount = 2;
        }
    }
    #endregion monobehaviour methods


    private Vector2 CheckRayHitPoint(Vector2 originPoint, Vector2 direction, float distance)
    {
        Ray2D ray = new Ray2D(originPoint, direction);
        RaycastHit2D rayHit = Physics2D.Raycast(ray.origin, ray.direction, distance);

        return Vector2.zero;//IMPLEMENT THIS LATER!

    }

    private void UpdateColliderBounds()
    {
        currentColliderBounds = new ColliderBounds();
        currentColliderBounds.bottomLeft = associatedCollider.bounds.min;
        currentColliderBounds.topRight = associatedCollider.bounds.max;
        currentColliderBounds.bottomRight = new Vector2();
    }


    #region structs
    private struct ColliderBounds
    {
        public Vector2 topLeft;
        public Vector2 topRight;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
    }
    #endregion structs
}
