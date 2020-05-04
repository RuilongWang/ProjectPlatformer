using System.Collections;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// The manager that handles how we control all of our physics objects in the game. This updates objects that contain a custom physics object and any collider object
/// </summary>
public class PhysicsManager : MonoBehaviour
{
    #region main variables
    public HashSet<CustomPhysics2D> AllActivePhysicsComponents = new HashSet<CustomPhysics2D>();
    #endregion main variables

    #region monobehaviour methods
    private void Awake()
    {
        
    }

    private void LateUpdate()
    {
        GameOverseer.Instance.HitboxManager.UpdateHitboxManager();
    }
    #endregion monobehaivour methods

    #region collection methods

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
