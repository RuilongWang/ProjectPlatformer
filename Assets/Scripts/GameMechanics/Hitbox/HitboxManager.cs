using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This class will manage all the hitboxes and hurtboxes that are associated with 
/// a character. Each hitbox and hurtbox object will also hold a reference to this object
/// </summary>
public class HitBoxManager : MonoBehaviour {

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

    /// <summary>
    /// Call this method whenever an enemy has been hit by a hitbox.
    /// The method will reutrn true if we made a successful attack (Meaning that we
    /// were able to damage to the enemy) and false if we did not make a successful attack on the enemy
    /// (either they were invincible or not able to get hit). Even if no damage is given, we will still return
    /// true if the attack langed
    /// </summary>
    /// <returns></returns>
    public virtual bool OnAttackedEnemyHurtbox(HitBox ourHitBox, HurtBox enemyHurtBox)
    {
        if (ourHitBox.associatedHitBoxManager == enemyHurtBox.associatedHiBboxManager)
        {
            return false;
        }
        if (allManagersEffectedByCurrentAttack.Contains(enemyHurtBox.associatedHiBboxManager))
        {
            return false;
        }

        enemyHurtBox.associatedHiBboxManager.associatedCharacterStats.TakeDamage(ourHitBox.damageToApply);
        return true;
    }

    /// <summary>
    /// Use this method if whenever we collide with an enemy hitbox. Returns false if nothing occurs
    /// upon colliding with an enemy hurtbox
    /// </summary>
    /// <param name="ourHitBox"></param>
    /// <param name="enemyHitbox"></param>
    /// <returns></returns>
    public virtual bool OnAttackedEnemyHitbox(HitBox ourHitBox, HitBox enemyHitbox)
    {

        return false;
    }



    public virtual void OnWasAttackedByEnemyHitbox(HurtBox ourHurtbox, HitBox enemyHitbox)
    {

    }

    #endregion event methods
}
