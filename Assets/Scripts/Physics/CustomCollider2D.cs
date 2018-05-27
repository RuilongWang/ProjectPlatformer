using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class is specifically used do detect collisions of kinematic objects in their environemnt. It will only 
/// ensure that an object does not pass through a InteractableTIle object
/// </summary>
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(CustomPhysics2D))]
public class CustomCollider2D : MonoBehaviour {
    public Collider2D associatedCollider { get; private set; }
    public float horizontalBuffer;
    public float verticalBuffer;

    [Header("Ray Counts")]
    [Tooltip("The number of rays we will fire in the horizontal direction")]
    public int horizontalRayCount = 4;
    [Tooltip("The number of rays we will fire in the vertical direction")]
    public int verticalRayCount = 4;


    private CustomPhysics2D rigid;
    private ColliderBounds currentColliderBounds;


    #region monobehaviour methods
    private void Awake()
    {
        rigid = GetComponent<CustomPhysics2D>();
        associatedCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        if (rigid)
        {
            rigid.allCustomColliders.Add(this);
        }
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

    private void OnDestroy()
    {
        if (rigid)
        {
            rigid.allCustomColliders.Remove(this);
        }
    }
    #endregion monobehaviour methods

    #region collision checks


    public void UpdateCollisionPhysics()
    {
        UpdateColliderBounds();
        //CheckCollisionUp();
    }

    private bool UpdateCollisionUp()
    {
        if (rigid.velocity.y <= 0)
        {
            return false;
        }

        List<TileCollider> tileCollidersThatWeHit = GetAllTilesHitFromRayCasts(
            currentColliderBounds.topLeft, currentColliderBounds.topRight, Vector2.up, Mathf.Abs(rigid.velocity.y), verticalRayCount);
        if (tileCollidersThatWeHit.Count == 0)
        {
            return false;
        }

        return true;
    }

    private bool UpdateCollisionDown()
    {
        if (rigid.velocity.y > 0)
        {
            return false;
        }

        List<TileCollider> tileCollidersThatWeHit = GetAllTilesHitFromRayCasts(
            currentColliderBounds.bottomLeft, currentColliderBounds.bottomRight, Vector2.down, Mathf.Abs(rigid.velocity.y), verticalRayCount);
        if (tileCollidersThatWeHit.Count == 0)
        {
            return false;
        }

        return true;
    }

    private bool UpdateCollisionRight()
    {
        if (rigid.velocity.x < 0)
        {
            return false;
        }

        List<TileCollider> tileCollidersThatWeHit = GetAllTilesHitFromRayCasts(
            currentColliderBounds.topRight, currentColliderBounds.bottomRight, Vector2.right, Mathf.Abs(rigid.velocity.x), horizontalRayCount);
        if (tileCollidersThatWeHit.Count == 0)
        {
            return false;
        }

        return true;
    }

    private bool UpdateCollisionLeft()
    {
        if (rigid.velocity.x >= 0)
        {
            return false;
        }

        List<TileCollider> tileCollidersThatWeHit = GetAllTilesHitFromRayCasts(
            currentColliderBounds.topLeft, currentColliderBounds.bottomLeft, Vector2.left, Mathf.Abs(rigid.velocity.x), horizontalRayCount);
        if (tileCollidersThatWeHit.Count == 0)
        {
            return false;
        }

        return true;

    }

    /// <summary>
    /// Gets a list of all environmental colliders hit given the following parameters.
    /// </summary>
    /// <param name="point1"></param>
    /// <param name="point2"></param>
    /// <param name="directionToCastRay"></param>
    /// <param name="distanceToCastRay"></param>
    /// <param name="totalPointsToCheck"></param>
    /// <returns></returns>
    private List<TileCollider> GetAllTilesHitFromRayCasts(Vector2 point1, Vector2 point2, Vector2 directionToCastRay, float distanceToCastRay, int totalPointsToCheck)
    {
        List<TileCollider> allCollidersThatWereHit = new List<TileCollider>();

        Vector2 segmentDistance = (point2 - point1) / (totalPointsToCheck - 1);

        Vector2 originPointForRaycast = point1;

        for (int i = 0; i < totalPointsToCheck; i++)
        {
            RaycastHit2D rayHit = Physics2D.Raycast(originPointForRaycast, directionToCastRay, distanceToCastRay, LayerMask.GetMask("Environment"));
            if (rayHit)
            {
                TileCollider tileColliderThatWasHit = rayHit.collider.GetComponent<TileCollider>();
                if (tileColliderThatWasHit)
                {
                    allCollidersThatWereHit.Add(tileColliderThatWasHit);
                }
            }
        }

        return allCollidersThatWereHit;
    }
    #endregion collision checks

    private void UpdateColliderBounds()
    {
        currentColliderBounds = new ColliderBounds();
        currentColliderBounds.bottomLeft = associatedCollider.bounds.min + Vector3.right * horizontalBuffer + Vector3.up * verticalBuffer;
        currentColliderBounds.topRight = associatedCollider.bounds.max - Vector3.right * horizontalBuffer - Vector3.up * verticalBuffer;
        currentColliderBounds.bottomRight = new Vector3(associatedCollider.bounds.max.x, associatedCollider.bounds.min.y) - Vector3.right * horizontalBuffer + Vector3.up * verticalBuffer;
        currentColliderBounds.topLeft = new Vector3(associatedCollider.bounds.min.x, associatedCollider.bounds.max.y) + Vector3.right * horizontalBuffer - Vector3.up * verticalBuffer;
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
