using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private Dictionary<HitboxInteraction, HashSet<Hitbox>> AllHitboxesInLevel = new Dictionary<HitboxInteraction, HashSet<Hitbox>>();
    private List<HitboxInteraction> ListOfAllHitboxInteractionComponents = new List<HitboxInteraction>();
    #region monobehaviour methods

    #endregion monobehaviour methods


    #region updating hitbox interactions
    /// <summary>
    /// Call this method once every frame to update the interactions between all of our hitboxes
    /// </summary>
    public void UpdateHitboxManager()
    {
        UpdateTheBoundsOfAllActiveHitboxes();
        CheckForAllOverlappingHitboxes();
    }

    /// <summary>
    /// Run this method to update the bounds of all valid hitboxes
    /// </summary>
    private void UpdateTheBoundsOfAllActiveHitboxes()
    {
        foreach (KeyValuePair<HitboxInteraction, HashSet<Hitbox>> HitboxInteraction in AllHitboxesInLevel)
        {
            foreach (Hitbox Hitbox in HitboxInteraction.Value)
            {
                if (ShouldHitboxBeUpdated(Hitbox))
                    Hitbox.UpdateHitboxBounds();
            }
        }
    }

    /// <summary>
    /// Lets us know if the hitbox should be checked or if we should skip it for interactions
    /// </summary>
    /// <param name="HitboxToCheck"></param>
    /// <returns></returns>
    private bool ShouldHitboxBeUpdated(Hitbox HitboxToCheck)
    {
        if (!HitboxToCheck.gameObject.activeInHierarchy) return false;
        return true;
    }

    #endregion updating hitbox interactions

    #region overlap methods
    /// <summary>
    /// 
    /// </summary>
    private void CheckForAllOverlappingHitboxes()
    {
        for (int i = 0; i < ListOfAllHitboxInteractionComponents.Count - 1; ++i)
        {
            for (int j = i + 1; j < ListOfAllHitboxInteractionComponents.Count; ++j)
            {
                HashSet<Hitbox> HitboxSet1 = AllHitboxesInLevel[ListOfAllHitboxInteractionComponents[i]];
                HashSet<Hitbox> HitboxSet2 = AllHitboxesInLevel[ListOfAllHitboxInteractionComponents[j]];
                CheckForOverlapInHitboxSetsAndLaunchAppropriateEvent(HitboxSet1, HitboxSet2);
            }
        }
    }

    /// <summary>
    /// Helper method to check that we are overlapping with the two passed in hitbox
    /// </summary>
    /// <param name="HitboxSet1"></param>
    /// <param name="HitboxSet2"></param>
    /// <returns></returns>
    private void CheckForOverlapInHitboxSetsAndLaunchAppropriateEvent(HashSet<Hitbox> HitboxSet1, HashSet<Hitbox> HitboxSet2)
    {
        foreach(Hitbox Hitbox1 in HitboxSet1)
        {
            foreach (Hitbox Hitbox2 in HitboxSet2)
            {
                if (Hitbox1.IsOverlappingHitboxAndValid(Hitbox2))
                {
                    if (!Hitbox1.IsHitboxCurrentlyRegisteredAsOverlapped(Hitbox2))
                    {
                        Hitbox1.OnHitboxBeginOverlap(Hitbox2);
                        Hitbox2.OnHitboxBeginOverlap(Hitbox1);
                    }
                    Hitbox1.OnHitboxOverlapStay(Hitbox2);
                    Hitbox2.OnHitboxOverlapStay(Hitbox1);
                }
                else if (Hitbox1.IsHitboxCurrentlyRegisteredAsOverlapped(Hitbox2))
                {
                    Hitbox1.OnHitboxEndOverlap(Hitbox2);
                    Hitbox2.OnHitboxEndOverlap(Hitbox1);
                }

            }
        }
    }
    #endregion overlap methods


    #region collection methods
    /// <summary>
    /// Safely Adds a hitbox to our hitbox manager 
    /// </summary>
    /// <param name="HitboxToAdd"></param>
    public void AddHitboxToManager(Hitbox HitboxToAdd)
    {
        if (!AllHitboxesInLevel.ContainsKey(HitboxToAdd.AssociatedHitboxInteractionComponent))
        {
            AllHitboxesInLevel.Add(HitboxToAdd.AssociatedHitboxInteractionComponent, new HashSet<Hitbox>());
            ListOfAllHitboxInteractionComponents.Add(HitboxToAdd.AssociatedHitboxInteractionComponent);
        }

        if (!AllHitboxesInLevel[HitboxToAdd.AssociatedHitboxInteractionComponent].Add(HitboxToAdd))
        {
            Debug.LogWarning("You are attempting to add a hitbox that was already added to our Htibox manager");
        }
    }

    /// <summary>
    /// Safely removes a hitbox from our hitbox manager
    /// </summary>
    /// <param name="HitboxToRemove"></param>
    public void RemoveHitboxFromManager(Hitbox HitboxToRemove)
    {
        if (!AllHitboxesInLevel.ContainsKey(HitboxToRemove.AssociatedHitboxInteractionComponent))
        {
            Debug.LogWarning("The associated hitbox interaction component was not found in our list. Please double check that you properly added this hitbox before calling remove");
            return;
        }

        if (!AllHitboxesInLevel[HitboxToRemove.AssociatedHitboxInteractionComponent].Remove(HitboxToRemove))
        {
            Debug.LogWarning("You are attemtping to remove a hitbox that was already added to our Hitbox manager");
        }
        if (AllHitboxesInLevel.Count <= 0)
        {
            AllHitboxesInLevel.Remove(HitboxToRemove.AssociatedHitboxInteractionComponent);
            ListOfAllHitboxInteractionComponents.Remove(HitboxToRemove.AssociatedHitboxInteractionComponent);
        }
    }
    #endregion collection methods

}
