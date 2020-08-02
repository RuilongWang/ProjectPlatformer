using JetBrains.Annotations;
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
    private Dictionary<CustomCollider2D, PhysicsColliderProperties> DictionaryOfIntersectedColliders = new Dictionary<CustomCollider2D, PhysicsColliderProperties>();
    
    #endregion main variables


    #region monobehaviour methods
    private void Awake()
    {
        
    }

    public void UpdatePhysicsManager()
    {
        UpdateGravityForPhysicsComponents();
        UpdateBoundsOfColliders();
        PushOutOverlappingPhysicsColliders();

        UpdateAllValidPhysicsComponents();
        ResetPhysicsToCollidersThatIntersected();

    }
    #endregion monobehaivour methods


    #region custom physics methods

    /// <summary>
    /// 
    /// </summary>
    private void ResetPhysicsToCollidersThatIntersected()
    {
        foreach (KeyValuePair<CustomCollider2D, PhysicsColliderProperties> ColliderKeyValue in DictionaryOfIntersectedColliders)
        {
            if (ColliderKeyValue.Value.CollidedVertically && ColliderKeyValue.Value.PreviousVelocity.y > 0)
            {
                ColliderKeyValue.Key.AssociatedPhysicsComponent.Velocity.y = ColliderKeyValue.Value.PreviousVelocity.y;
            }
            if (ColliderKeyValue.Value.CollidedHorizontally && ColliderKeyValue.Key.AssociatedPhysicsComponent.Velocity.y == 0)
            {
                ColliderKeyValue.Key.AssociatedPhysicsComponent.Velocity.x = ColliderKeyValue.Value.PreviousVelocity.x;
            }
        }
    }


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
            if (KeyValueColliderDictionaryData.Key != CustomCollider2D.ECollisionType.STATIC)
            {
                foreach (CustomCollider2D Collider2D in KeyValueColliderDictionaryData.Value)
                {
                    Collider2D.UpdateColliderBounds();
                }
            }
            
        }

        foreach (CustomCollider2D PhysicsCollider in AllActiveCollider2DComponentsInLevel[CustomCollider2D.ECollisionType.PHYSICS])
        {
            PhysicsCollider.UpdatePhysicsColliderBounds();
        }
    }


    /// <summary>
    /// Checks to see
    /// </summary>
    private void PushOutOverlappingPhysicsColliders()
    {
        DictionaryOfIntersectedColliders.Clear();
        PushOutOverlappingPhysicsCollliderByCategory(CustomCollider2D.ECollisionType.STATIC);
        PushOutOverlappingPhysicsCollliderByCategory(CustomCollider2D.ECollisionType.MOVABLE);
    }

    /// <summary>
    /// Helper method to push out physics colliders out of all colliders of the passed in collider type
    /// </summary>
    /// <param name="CollisionTypeToPushOut"></param>
    private void PushOutOverlappingPhysicsCollliderByCategory(CustomCollider2D.ECollisionType CollisionTypeToPushOut)
    {
        Vector3 ClosestCollisionOffset;
        Vector3 CollisionOffset;
        bool DidOverlapWithAnyCollider;
        bool IsOverlapValid; //
        PhysicsColliderProperties ColliderProperties;
        foreach (CustomCollider2D PhysicsCollider in AllActiveCollider2DComponentsInLevel[CustomCollider2D.ECollisionType.PHYSICS])
        {
            ClosestCollisionOffset = Vector2.zero;
            DidOverlapWithAnyCollider = false;
            foreach (CustomCollider2D StaticCollider in AllActiveCollider2DComponentsInLevel[CollisionTypeToPushOut])
            {
                
                IsOverlapValid = PhysicsCollider.IsPhysicsColliderOverlapping(StaticCollider);
                if (!Physics2D.GetIgnoreLayerCollision(PhysicsCollider.CollisionLayer, StaticCollider.CollisionLayer) &&
                    IsOverlapValid)
                {
                    if (!DictionaryOfIntersectedColliders.ContainsKey(PhysicsCollider))
                    {
                        DictionaryOfIntersectedColliders.Add(PhysicsCollider, new PhysicsColliderProperties());
                        DictionaryOfIntersectedColliders[PhysicsCollider].PreviousVelocity = PhysicsCollider.AssociatedPhysicsComponent.Velocity;
                    }

                    bool ShouldPushOutVertically, ShouldPushOutHorizontally;
                    CollisionOffset = StaticCollider.PushOutCollider(PhysicsCollider, out ShouldPushOutVertically, out ShouldPushOutHorizontally, true);
                    if (!DidOverlapWithAnyCollider)
                    {
                        ClosestCollisionOffset = CollisionOffset;
                        DidOverlapWithAnyCollider = true;
                        if (ShouldPushOutHorizontally) PhysicsCollider.AssociatedPhysicsComponent.Velocity.x = 0;
                        if (ShouldPushOutVertically) PhysicsCollider.AssociatedPhysicsComponent.Velocity.y = 0;
                    }
                    else
                    {
                        Vector2 PhysicsColliderCenterPoint = PhysicsCollider.GetAssociatedBounds().CenterPoint;
                        if (ShouldPushOutHorizontally)
                        {
                            float OriginalClosestX = Mathf.Abs(ClosestCollisionOffset.x - PhysicsColliderCenterPoint.x);
                            float NewPointToCheck = Mathf.Abs(CollisionOffset.x - PhysicsColliderCenterPoint.x);
                            if (NewPointToCheck < OriginalClosestX) { ClosestCollisionOffset.x = CollisionOffset.x; }
                            PhysicsCollider.AssociatedPhysicsComponent.Velocity.x = 0;
                        }
                        if (ShouldPushOutVertically)
                        {
                            float OriginalClosestY = Mathf.Abs(ClosestCollisionOffset.y - PhysicsColliderCenterPoint.y);
                            float NewPointYToCheck = Mathf.Abs(CollisionOffset.y - PhysicsColliderCenterPoint.y);
                            if (NewPointYToCheck < OriginalClosestY) { ClosestCollisionOffset.y = CollisionOffset.y; }
                            PhysicsCollider.AssociatedPhysicsComponent.Velocity.y = 0;
                        }
                    }

                    

                    ColliderProperties = DictionaryOfIntersectedColliders[PhysicsCollider];
                    ColliderProperties.CollidedHorizontally = ColliderProperties.CollidedHorizontally || ShouldPushOutHorizontally;
                    ColliderProperties.CollidedVertically = ColliderProperties.CollidedVertically || ShouldPushOutVertically;

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

    #region trigger events
    /// <summary>
    /// 
    /// </summary>
    private void GenerateOverlapEvents()
    {

    }
    #endregion trigger events


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

    #region helper classes
    private class PhysicsColliderProperties
    {
        public Vector2 PreviousVelocity;
        public bool CollidedHorizontally;
        public bool CollidedVertically;
    }
    #endregion helper classes
}
