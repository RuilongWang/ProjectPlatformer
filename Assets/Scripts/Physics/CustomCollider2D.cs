using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CustomCollider2D : MonoBehaviour {
    public Collider2D associatedCollider;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    private ColliderBounds currentColliderBounds;


    #region monobehaviour methods
    private void Start()
    {
        associatedCollider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        
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
