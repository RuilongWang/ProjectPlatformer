using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

/// <summary>
/// Base class for all types of hitboxes.
/// </summary>
public abstract class Hitbox : MonoBehaviour
{
    #region enums
    public enum HitboxType
    {
        Hitbox,
        Hurtbox,
    }
    #endregion enums

    #region debug references
    public Color DebugDrawColor
    {
        get
        {
            if (AllOverlappingHitboxes.Count > 0)
                return new Color(1, 0, 1);
            switch (AssignedHitboxType)
            {
                case HitboxType.Hitbox:
                    return Color.red;
                case HitboxType.Hurtbox:
                    return Color.cyan;
                default:
                    return Color.white;
            }
        }
    }
    #endregion debug references

    #region hitbox variables
    /// <summary>
    /// Every Actor that uses hitboxes should use this component. This is what we sue to properly
    /// organize and send off hitbox overlap events
    /// </summary>
    public HitboxInteraction AssociatedHitboxInteractionComponent { get; private set; }
    [Tooltip("We can use this to label the type of hitbox we are registering.")]
    public HitboxType AssignedHitboxType;
    protected HashSet<Hitbox> AllOverlappingHitboxes = new HashSet<Hitbox>();
    private CollisionFactory.Bounds AssociatedBounds;//Every hitbox will contain a generic reference to a bounds object
    #endregion hitbox variables

    #region monobehaviour methods
    protected virtual void Awake()
    {
        
        AssociatedHitboxInteractionComponent = GetComponentInParent<HitboxInteraction>();
        if (AssociatedHitboxInteractionComponent == null)
        {
            Debug.LogError("There was no hitbox interaction component assigned to a parent of our hitbox. Destroying Object...");
            Destroy(this.gameObject);
            return;
        }

        GameOverseer.Instance.HitboxManager.AddHitboxToManager(this);
    }

    private void OnDisable()
    {
        ClearAllOverlappingHitboxes();
    }


    protected virtual void OnDestroy()
    {
        ClearAllOverlappingHitboxes();
        if (GameOverseer.Instance && GameOverseer.Instance.HitboxManager)
            GameOverseer.Instance.HitboxManager.RemoveHitboxFromManager(this);
    }
    #endregion monobehaviour methods
    public abstract void UpdateHitboxBounds();

    /// <summary>
    /// This mehtod must be called to properly check for collisions
    /// </summary>
    /// <param name="BoundsToAssign"></param>
    public void AssignHitboxBounds(CollisionFactory.Bounds BoundsToAssign)
    {
        this.AssociatedBounds = BoundsToAssign;
    }

    /// <summary>
    /// This method checks to see if we are overlapping a hitbox and if that overlap is valid. If we return
    /// true then it is defined as a valid hitbox interaction
    /// </summary>
    /// <param name="OtherHitbox"></param>
    /// <returns></returns>
    public bool IsOverlappingHitboxAndValid(Hitbox OtherHitbox)
    {
        //We return false if the hitbox we are checking against comes from the same actor
        if (this.AssociatedHitboxInteractionComponent == OtherHitbox.AssociatedHitboxInteractionComponent)
        {
            return false;
        }

        //If the hitboxes are the same type we also do not register it as a valid overlap
        if (this.AssignedHitboxType == OtherHitbox.AssignedHitboxType)
        {
            return false;
        }

        return this.AssociatedBounds.IsOverlappingBounds(OtherHitbox.AssociatedBounds);
    }

    /// <summary>
    /// Returns true if this hitbox is already registered as being overlapped.
    /// </summary>
    /// <param name="OtherHitbox"></param>
    /// <returns></returns>
    public bool IsHitboxCurrentlyRegisteredAsOverlapped(Hitbox OtherHitbox)
    {
        return AllOverlappingHitboxes.Contains(OtherHitbox);
    }

    /// <summary>
    /// This method will remove all Overlapping hitboxes and call on end overlap for all hitboxes that were previously
    /// overlapping with it
    /// </summary>
    private void ClearAllOverlappingHitboxes()
    {
        foreach (Hitbox OverlappingHitbox in AllOverlappingHitboxes)
        {
            OverlappingHitbox.OnHitboxEndOverlap(this);

        }
        AllOverlappingHitboxes.Clear();//On overlap end will not be called when we are clearing it
    }


    /// <summary>
    /// This method will be called whenever out hitbox begins the overlap process with another hitbox
    /// </summary>
    public void OnHitboxBeginOverlap(Hitbox OtherHitbox)
    {
        if (!AllOverlappingHitboxes.Add(OtherHitbox))
        {
            Debug.LogWarning("You are trying to begin an overlap event with a hitbox that has already been registered");
            return;
        }
        AllOverlappingHitboxes.Add(OtherHitbox);
    }

    /// <summary>
    /// This method will be called anytime we end an overlap with a hitbox that we were overlapping previously
    /// NOTE: Keep in mind that this will also be called when a hitbox has been disabled.
    /// </summary>
    /// <param name="OtherHitbox"></param>
    public void OnHitboxEndOverlap(Hitbox OtherHitbox)
    {
        if (!AllOverlappingHitboxes.Remove(OtherHitbox))
        {
            Debug.LogWarning("You are trying to end an overlap event with a hitbox that was not registered as overlapped");
            return;
        }
        AllOverlappingHitboxes.Remove(OtherHitbox);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="OtherHitbox"></param>
    public void OnHitboxOverlapStay(Hitbox OtherHitbox)
    {
        
    }

}
