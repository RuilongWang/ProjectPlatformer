using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// </summary>
public class HitboxManager : MonoBehaviour
{
    /// <summary>
    /// A collection of all the hitboxes the are currently active in our scene. When running calculations for intersecions we will skip over
    /// hitboxes that are disabled or attached to disabled gameobjects
    /// </summary>
    private HashSet<Hitbox> AllHitboxesInLevel;

    #region monobehaviour methods

    #endregion monobehaviour methods


    #region updating hitbox interactions
    /// <summary>
    /// Call this method once every frame to update the interactions between all of our hitboxes
    /// </summary>
    public void UpdateHitboxManager()
    {

    }

    private void UpdateTheBoundsOfAllActiveHitboxes()
    {
        foreach(Hitbox hitbox in AllHitboxesInLevel)
        {
            hitbox.UpdateHitboxBounds();
        }
    }
    #endregion updating hitbox interactions

    #region collection methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="HitboxToAdd"></param>
    public void AddHitboxToManager(Hitbox HitboxToAdd)
    {
        if (!AllHitboxesInLevel.Add(HitboxToAdd))
        {
            Debug.LogWarning("You are attempting to add a hitbox that was already added to our Htibox manager");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="HitboxToRemove"></param>
    public void RemoveHitboxFromManager(Hitbox HitboxToRemove)
    {
        if (!AllHitboxesInLevel.Remove(HitboxToRemove))
        {
            Debug.LogWarning("You are attemtping to remove a hitbox that was already added to our Hitbox manager");
        }
    }
    #endregion collection methods

}
