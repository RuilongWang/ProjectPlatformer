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
    #endregion const variables


    #region main variables
    [Tooltip("The character stats that are associated with this hitbox manager. Any damage or effects taht are applied to this object will reference this character stats object")]
    public CharacterStats associatedCharacterStats;
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
    private void Start()
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
    #endregion event methods
}
