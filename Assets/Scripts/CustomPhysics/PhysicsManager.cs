using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// The manager that handles how we control all of our physics objects in the game. This updates objects that contain a custom physics object and any collider object
/// </summary>
public class PhysicsManager : MonoBehaviour
{
    #region main variables
    /// <summary>
    /// 
    /// </summary>
    public HashSet<CustomPhysics2D> AllActivePhysicsComponents = new HashSet<CustomPhysics2D>();

    /// <summary>
    /// 
    /// </summary>
    public Dictionary<CustomCollider2D.ECollisionType, HashSet<CustomCollider2D>> AllActiveCollider2DComponentsInLevel = new Dictionary<CustomCollider2D.ECollisionType, HashSet<CustomCollider2D>>();
    #endregion main variables


    #region monobehaviour methods
    private void Awake()
    {
        
    }

    private void LateUpdate()
    {
        GameOverseer.Instance.HitboxManager.UpdateHitboxManager();
        UpdateGravityForPhysicsComponents();
        UpdateBoundsOfColliders();
        CheckOfrOverlappingColliders();
        UpdateAllValidPhysicsComponents();
    }
    #endregion monobehaivour methods


    #region custom physics methods
    private void UpdateGravityForPhysicsComponents()
    {
        foreach (CustomPhysics2D PhysicsComponent in AllActivePhysicsComponents)
        {
            if (PhysicsComponent.gameObject.activeInHierarchy)
            {
                PhysicsComponent.UpdateVelocityFromGravity();
            }
        }
    }

    private void UpdateBoundsOfColliders()
    {
        foreach (KeyValuePair<CustomCollider2D.ECollisionType, HashSet<CustomCollider2D>> KeyValueColliderDictionaryData in AllActiveCollider2DComponentsInLevel)
        {
            foreach (CustomCollider2D Collider2D in KeyValueColliderDictionaryData.Value)
            {
                Collider2D.UpdateColliderBounds();
            }
        }

        foreach (CustomCollider2D PhysicsCollider in AllActiveCollider2DComponentsInLevel[CustomCollider2D.ECollisionType.PHYSICS])
        {
            PhysicsCollider.UpdatePhysicsColliderBounds();
        }
    }

    private void CheckOfrOverlappingColliders()
    {
        Vector3 ClosestCollisionOffset;
        Vector3 CollisionOffset;
        bool DidOverlapWithAnyCollider;
        foreach (CustomCollider2D PhysicsCollider in AllActiveCollider2DComponentsInLevel[CustomCollider2D.ECollisionType.PHYSICS])
        {
            ClosestCollisionOffset = Vector2.zero;
            DidOverlapWithAnyCollider = false;
            foreach (CustomCollider2D StaticCollider in AllActiveCollider2DComponentsInLevel[CustomCollider2D.ECollisionType.STATIC])
            {
                if (!Physics2D.GetIgnoreLayerCollision(PhysicsCollider.CollisionLayer, StaticCollider.CollisionLayer) &&
                    PhysicsCollider.IsPhysicsColliderOverlapping(StaticCollider))
                {
                    bool ShouldPushOutVertically, ShouldPushOutHorizontally;
                    CollisionOffset = StaticCollider.PushOutCollider(PhysicsCollider, out ShouldPushOutVertically, out ShouldPushOutHorizontally, true);
                    if (!DidOverlapWithAnyCollider)
                    {
                        ClosestCollisionOffset = CollisionOffset;
                    }
                    else
                    {
                        Vector2 PhysicsColliderCenterPoint = PhysicsCollider.GetAssociatedBounds().CenterPoint;
                        if (ShouldPushOutHorizontally)
                        {
                            float OriginalClosestX = Mathf.Abs(ClosestCollisionOffset.x - PhysicsColliderCenterPoint.x);
                            float NewPointToCheck = Mathf.Abs(CollisionOffset.x - PhysicsColliderCenterPoint.x);
                            if (NewPointToCheck < OriginalClosestX) { ClosestCollisionOffset.x = CollisionOffset.x; }
                        }
                        if (ShouldPushOutVertically)
                        {
                            float OriginalClosestY = Mathf.Abs(ClosestCollisionOffset.y - PhysicsColliderCenterPoint.y);
                            float NewPointYToCheck = Mathf.Abs(CollisionOffset.y - PhysicsColliderCenterPoint.y);
                            if (NewPointYToCheck < OriginalClosestY) { ClosestCollisionOffset.y = CollisionOffset.y; }
                        }
                    }
                    DidOverlapWithAnyCollider = true;
                }
                
            }
            if (DidOverlapWithAnyCollider)
            {
                PhysicsCollider.transform.position += ClosestCollisionOffset;
            }
        }
    }



    /// <summary>
    /// 
    /// </summary>
    private void UpdateAllValidPhysicsComponents()
    {
        foreach (CustomPhysics2D PhysicsComponent in AllActivePhysicsComponents)
        {
            if (PhysicsComponent.gameObject.activeInHierarchy)
            {
                PhysicsComponent.UpdatePhysics();
            }
        }
    }
    #endregion custom physics methods


    #region collection methods
    /// <summary>
    /// Safely Adds a collider2D to our Physics manager
    /// </summary>
    /// <param name="Collider2D"></param>
    public void AddCollider2DToPhysicsManager(CustomCollider2D Collider2D)
    {
        if (!AllActiveCollider2DComponentsInLevel.ContainsKey(Collider2D.AssignedCollisionType))
        {
            AllActiveCollider2DComponentsInLevel.Add(Collider2D.AssignedCollisionType, new HashSet<CustomCollider2D>());
        }
        if (!AllActiveCollider2DComponentsInLevel[Collider2D.AssignedCollisionType].Add(Collider2D))
        {
            Debug.LogWarning("You are trying to add a collider that was already added to our list.");
        }
    }

    /// <summary>
    /// Safely removes a collider from our Physics manager
    /// </summary>
    /// <param name="Collider2D"></param>
    public void RemoveCollider2DFromPhysicsManager(CustomCollider2D Collider2D)
    {
        if (!AllActiveCollider2DComponentsInLevel.ContainsKey(Collider2D.AssignedCollisionType))
        {
            Debug.LogWarning("You are trying to remove a collider before the collision type has been added. Something may have gone wrong here");
            return;
        }

        if (!AllActiveCollider2DComponentsInLevel[Collider2D.AssignedCollisionType].Remove(Collider2D))
        {
            Debug.LogWarning("There was no collider found for this Collider2D object. Either you already removed this object or you are removing something that was never added.");
        }
    }

    public void AddCustomPhysics(CustomPhysics2D PhysicsComponent)
    {
        if (!AllActivePhysicsComponents.Add(PhysicsComponent))
        {
            Debug.LogWarning("You are trying to add a physics component that was already assigned to our Physics Manager.");
            return;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="PhysicsComponent"></param>
    /// <returns></returns>
    public void RemoveCustomPhysics(CustomPhysics2D PhysicsComponent)
    {
        if (!AllActivePhysicsComponents.Remove(PhysicsComponent))
        {
            Debug.LogWarning("The physics component that was passed in was not found in our physics manager. This may mean that you already removed it or it was never added.");
            return;
        }
    }
    #endregion collection methods

}
