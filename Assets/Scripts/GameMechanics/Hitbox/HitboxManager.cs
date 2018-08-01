using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class will manage all the hitboxes and hurtboxes that are associated with 
/// a character. Each hitbox and hurtbox object will also hold a reference to this object
/// </summary>
public class HitBoxManager : MonoBehaviour {
    #region const variables
    public const string HITBOX_LAYER = "Hitbox";
    public const string ENVIRONMENT_LAYER = "Environment";
    #endregion const variables


    #region main variables
    /// <summary>
    /// The character stats that are associated with this hitbox manager. Any damage or effects that are 
    /// applied to this object will reference this character stats object.
    /// </summary>
    public CharacterStats associatedCharacterStats { get; set; }


    /// <summary>
    /// This variable inicates that an enemy is currently in hitstun. Meaning that they can not move or take any
    /// control of their character until they have recovered back down to 0
    /// </summary>
    public float timeRemainingForHitStun { get; private set; }


    private List<HitBoxManager> allManagersEffectedByCurrentAttack = new List<HitBoxManager>();
    [System.NonSerialized]
    public List<HitBox> allConnectedHitboxes = new List<HitBox>();
    [System.NonSerialized]
    public List<HurtBox> allConnectedHurtboxes = new List<HurtBox>();
    #endregion main variables

    #region monbehavour methods
    protected virtual void Start()
    {
        this.associatedCharacterStats = GetComponentInParent<CharacterStats>();
    }
    #endregion monobehaviour methods

    #region event methods
    /// <summary>
    /// Call this method whenever there is a new attack that has occurred.
    /// It will reset the necessary values. This would be best to call in an animator, to ensure that a new  
    /// </summary>
    public void OnResetHitBoxManager()
    {
        allManagersEffectedByCurrentAttack.Clear();
    }

    /// <summary>
    /// If we already hit an object with a hitbox, we may not want to hit it again within the same animation. In which case, we can use this
    /// method to check if we have already hit them
    /// </summary>
    /// <param name="hitboxManager"></param>
    /// <returns></returns>
    protected bool ContainsManager(HitBoxManager hitboxManager)
    {
        foreach (HitBoxManager hManager in allManagersEffectedByCurrentAttack)
        {
            if (hManager == hitboxManager)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// If this is called it will disable all the hitboxes and hurboxes that are attached to our manager
    /// </summary>
    public virtual void DeactivateHitboxManager()
    {
        foreach (HitBox hbox in allConnectedHitboxes)
        {
            hbox.gameObject.SetActive(false);
        }
        foreach(HurtBox hbox in allConnectedHurtboxes)
        {
            hbox.gameObject.SetActive(false);
        }
        this.enabled = false;
    }

    /// <summary>
    /// Enables the hitbox manager if it was turned off
    /// </summary>
    public virtual void ActivateHitboxManager()
    {
        this.enabled = true;
    }
    #endregion event methods
}
